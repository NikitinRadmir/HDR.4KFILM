using System.Net;
using System.Text;
using System.Threading.Channels;
using HttpServerLibrary.Core;
using HttpServerLibrary.Handlers;

namespace HttpServerLibrary;
/// <summary>
/// Класс HTTP сервера реализуйющего получение загаловков, старт сервера, остановкку сервера, получение контекста, обработку запроса
/// работает на основе цеаочки обязанностей Handler'ов наследующих <see cref="Handler"/>
/// </summary>
public class HttpServerWithTypes
{
    /// <summary>
    /// <see cref="HttpListener"/> лисенер прослушивающих запросы на префиксах
    /// </summary>
    private HttpListener _listener;
    /// <summary>
    /// путь до статических файлов
    /// </summary>
    private static string _staticDirectoryPath;
    /// <summary>
    /// <see cref="StaticFileHandler"/> обработчик запросов на статические файлы
    /// </summary>
    private readonly Handler _staticFileHandler = new StaticFileHandler();
    /// <summary>
    /// <see cref="EndpointsHandler"/>
    /// обработчик Endpoint'ов
    /// </summary>
    private readonly Handler _endpointsHandler = new EndpointsHandler();

    private readonly Handler _notFoundHandler = new NotFoundHandler();
    /// <summary>
    /// Обрабатывает префиксы, инициализирует <see cref="_listener"/>
    /// </summary>
    /// <param name="prefixes">Префиксы которые будет слушать <see cref="_listener"/></param>
    /// <param name="staticDirectoryPath">Путь до статических файлов</param>
    public HttpServerWithTypes(string[] prefixes, string staticDirectoryPath)
    {
        _listener = new HttpListener();
        foreach (var prefix in prefixes)
        {
            _listener.Prefixes.Add(prefix);
            Console.WriteLine(prefix);
        }

        _staticDirectoryPath = staticDirectoryPath;
    }
    /// <summary>
    /// Метод для старта работы сервера, вызывает Start() из <see cref="HttpListener"/>
    /// запускает ожидание контекста
    /// </summary>
    public void Start()
    {
        _listener.Start();
        Console.WriteLine("Server started");
        Receive();
    }

    /// <summary>
    /// Останавливает работу сервера
    /// </summary>
    public void Stop()
    {
        _listener.Stop();
        Console.WriteLine("Server stopped");
    }

    /// <summary>
    /// Метод для получения контекста
    /// вызвает обработку контекста
    /// работает все время пока <see cref="_listener"/>
    /// слушает префиксы
    /// </summary>
    private async void Receive()
    {
        while (_listener.IsListening)
        {
            var context = _listener.GetContext();
            HandleRequest(context);
            //Console.WriteLine("Receiving context");
        }
    }

    /// <summary>
    /// метод обработки пришедшего контекста
    /// запускает работу классов из цепочки обязанностей
    /// по сути еще должен ошибки отлавливать, но пока без этого :)
    /// </summary>
    /// <param name="context"></param>
    private void HandleRequest(HttpListenerContext context)
    {
        
        _staticFileHandler.Successor = _endpointsHandler; //назначаем следующий обработчик для staticFileHandler
        var requestContext = new HttpRequestContext(context.Request, context.Response); 
        _staticFileHandler.HandleRequest(requestContext);
        _endpointsHandler.Successor = _notFoundHandler;
        //TODO: Сделать правильно (пока не знаю как правильно)
        // try
        // {
        //     _staticFileHandler.Successor = _endpointsHandler;
        //     var requestContext = new HttpRequestContext(context.Request, context.Response);
        //     _staticFileHandler.HandleRequest(requestContext);
        // }
        // catch (Exception e)
        // {
        //     switch (e.Message)
        //     {
        //         case "Одинаковые Роутинги":
        //             Stop();
        //             Console.WriteLine(e.Message);
        //             break;
        //         case "Меньше параметров чем требуется":
        //             Console.WriteLine(e.Message);
        //             break;
        //     }
        //     
        // }
    }
}