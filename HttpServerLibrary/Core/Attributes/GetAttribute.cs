namespace HttpServerLibrary.Core.Attributes;

/// <summary>
/// Атрибут для получения метода обрабытвающего Get запросы
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class GetAttribute : Attribute
{
    /// <summary>
    /// Получение роута для Get запроса
    /// </summary>
    public string Route { get;}

    /// <summary>
    /// Инициализазация нового экземпляра GetAttribute с полученным роутом
    /// </summary>
    /// <param name="route">Роут по которому находится метод вызываемый Get запросом</param>
    public GetAttribute(string route)
    {
        Route = route;
    }
}