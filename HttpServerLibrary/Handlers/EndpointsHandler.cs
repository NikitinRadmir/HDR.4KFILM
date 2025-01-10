using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Reflection;
using System.Text;
using HttpServerLibrary.Core;
using HttpServerLibrary.Core.Attributes;
using HttpServerLibrary.Core.HttpResponse;


namespace HttpServerLibrary.Handlers;
/// <summary>
/// Класс обрабатывающий и регистрирующий Endpoint'ы, вызывающий соответвующие пришедшим запросам методы,
/// реализует функционал маршрутизации запросов наследующих <see cref="EndpointBase"/>
/// </summary>
public sealed class EndpointsHandler : Handler
{
    /// <summary>
    /// Словарь зарегистрированных роутов. Ключом выступает роутинг значением список методов, информации о методах и
    /// Endpoint'ов связанных с этим роутингом
    /// </summary>
    private readonly Dictionary<string, List<(HttpMethod method, MethodInfo methodInfo, Type endpointType)>> _routes = new ();

    /// <summary>
    /// Вызывает <see cref="RegisterEndpointsFromAssemblies"/> для регистрации всех Endpoint'ов
    /// </summary>
    public EndpointsHandler()
    {
        RegisterEndpointsFromAssemblies(new[]{Assembly.GetEntryAssembly()});
    }
    /// <summary>
    /// Метод обработки запроса, определяет по какому роутингу пришел запрос, находит для него соответсвующий метод
    /// вызывает его с требуемыми параметрами
    /// в случае отсутствия искомого роутинка передает запрос в <see cref="NotFoundHandler"/>
    /// </summary>
    /// <param name="context">контекст текущего метода</param>
    public override void HandleRequest(HttpRequestContext context)
    {
        // некоторая обработка запроса
        var request = context.Request;
        var url = context.Request.Url.LocalPath.Trim('/');
        var requestMethod = context.Request.HttpMethod;
        if(_routes.ContainsKey(url))
        {
            // получение роутинга из словаря зарегистрированных роутингов
            var route = _routes[url].FirstOrDefault(r => r.method.ToString().Equals(requestMethod, StringComparison.OrdinalIgnoreCase));
            if (route.methodInfo != null)
            {
                // создаем instance Endpoint'a
                var endpointInstance = Activator.CreateInstance(route.endpointType) as EndpointBase;
                if (endpointInstance != null)
                {
                    endpointInstance.SetContext(context);
                    // вызываем метод
                    var parameters = GetMethodParameters(route.methodInfo, context); //try catch (response 500)
                    var result = route.methodInfo.Invoke(endpointInstance, parameters) as IHttpResponseResult;
                    result?.Execute(context);
                }
            }
        }
        // передача запроса дальше по цепи при наличии в ней обработчиков
        else if (Successor != null)
        {
            
            Successor.HandleRequest(context);
        }
    }
    
    /// <summary>
    /// Метод регистрации Endpoint'ов из соборок получаемых в параметрах
    /// получает все Endpoint'ы наследующие <see cref="EndpointBase"/>
    /// вытаскивает из них методы и регистрирует их в соответствии с их типом и роутингом, используя
    /// <see cref="RegisterRoute"/>
    /// </summary>
    /// <param name="assemblies"></param>
    private void RegisterEndpointsFromAssemblies(Assembly[] assemblies)
    {
        foreach (Assembly assembly in assemblies)
        {
            var endpointTypes = assembly.GetTypes()
                .Where(t => typeof(EndpointBase).IsAssignableFrom(t) && !t.IsAbstract);
            foreach (var endpointType in endpointTypes)
            {
                var methods = endpointType.GetMethods();
                foreach (var methodInfo in methods)
                {
                    //Можно отрефакторить
                    var getAttribute = methodInfo.GetCustomAttribute<GetAttribute>();
                    if (getAttribute != null)
                    {
                        RegisterRoute(getAttribute.Route.ToLower(), HttpMethod.Get, methodInfo, endpointType);
                    }

                    var postAttribute = methodInfo.GetCustomAttribute<PostAttribute>(); //wrong attribute type
                    if (postAttribute != null)
                    {
                        RegisterRoute(postAttribute.Route.ToLower(), HttpMethod.Post, methodInfo, endpointType);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Метод регистрации роутинга
    /// вносит данные о роутинге в словарь <see cref="_routes"/>
    /// </summary>
    /// <param name="route">Роутинг (URL)</param>
    /// <param name="method">Тип запроса (HttpMetod)</param>
    /// <param name="methodInfo">Метод в Endpoint'е который соответсвует этому роутингу</param>
    /// <param name="endpointType">Тип Endpoint'a </param>
    /// <exception cref="Exception">Выбрасывается если есть два метода соответствующих одному роутингу</exception>
    private void RegisterRoute(string route, HttpMethod method, MethodInfo methodInfo, Type endpointType)
    {
        
        //TODO добавить проверку на одинаковые роутинги и одинаковые HttpMethod, выкидывать исключение
        //с сообщением в консоль и прекратить работу сервера
        if (!_routes.ContainsKey(route))
        {
            _routes[route] = new ();
        }
        else if (_routes[route].Any(x => x.method == method))
        {
            throw new Exception("Одинаковые Роутинги"); //потом переделаю, пока не в приоритете
        }

        _routes[route].Add((method, methodInfo, endpointType));

    }
    /// <summary>
    /// Обрабытвает параметры запроса из строки запроса или тела запроса
    /// подготавливает их для отправки в запрос
    /// </summary>
    /// <param name="method">Метод в который буду направляться параметры</param>
    /// <param name="context">Контекст текущего запроса</param>
    /// <returns>возвращает коллекцию ключ-значение параметров для метода </returns>
    /// <exception cref="Exception"></exception>
    private object[] GetMethodParameters(MethodInfo method, HttpRequestContext context)
        {
            var parameters = method.GetParameters();
            var values = new object[parameters.Length];

            if (context.Request.HttpMethod.Equals("GET", StringComparison.InvariantCultureIgnoreCase))
            {
                // Извлекаем параметры из строки запроса
                var queryParameters = System.Web.HttpUtility.ParseQueryString(context.Request.Url.Query);
                for (int i = 0; i < parameters.Length; i++) //проходимся по все полученным параметрам, приводим их к типу требуемому в method, записываем их в результируюий массив
                {
                    var paramName = parameters[i].Name;
                    var paramType = parameters[i].ParameterType;
                    var value = queryParameters[paramName];
                    values[i] = ConvertValue(value, paramType);
                }
            }
            else if (context.Request.HttpMethod.Equals("POST", StringComparison.InvariantCultureIgnoreCase))
            {
                // Извлекаем параметры из тела запроса
                using var reader = new StreamReader(context.Request.InputStream);
                var body = reader.ReadToEnd();
                // Если данные переданы в формате application/x-www-form-urlencoded,
                // преобразуем тело запроса в коллекцию параметров.
                if (context.Request.ContentType == "application/x-www-form-urlencoded")
                {
                    var formParameters = System.Web.HttpUtility.ParseQueryString(body);
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        var paramName = parameters[i].Name;
                        var paramType = parameters[i].ParameterType;
                        var value = formParameters[paramName];
                        values[i] = ConvertValue(value, paramType);
                    }
                }
                else if (context.Request.ContentType == "application/json")
                {
                    // Парсим JSON в объект
                    var jsonObject =
                        System.Text.Json.JsonSerializer.Deserialize(body, method.GetParameters()[0].ParameterType);
                    return new[] { jsonObject };
                }
            }
            //TODO: Переработать логику обработки несовпадения переданных в запросе значений и параметров метода
            if (!AreParametersCorrespondence(parameters, values)) throw new Exception("Меньше параметров чем требуется");

            return values;
        }
    /// <summary>
    /// Приводит значение к требуемому типу
    /// </summary>
    /// <param name="value">значение</param>
    /// <param name="targetType">Требуемый тип</param>
    /// <returns>Значение приведенное к нужному типу</returns>
    private object ConvertValue(string value, Type targetType)
    {
        if (value == null)
        {
            return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
        }

        return Convert.ChangeType(value, targetType);
    }

    /// <summary>
    /// Проверяет соответсвие количества полученных значений требуемым параметрам
    /// </summary>
    /// <param name="parameters">требуемые параметры</param>
    /// <param name="values">полученные значение</param>
    /// <returns>True если количество значений соответсвует и Falsr иначе</returns>
    private bool AreParametersCorrespondence(ParameterInfo[] parameters, object[] values)
        => parameters.Length <= values.Select(x => x).Where(v => v != null).ToArray().Length;

}