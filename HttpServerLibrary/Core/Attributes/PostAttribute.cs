namespace HttpServerLibrary.Core.Attributes;
/// <summary>
/// Атрибут для получения метода обрабытвающего Post запросы
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class PostAttribute : Attribute
{
    /// <summary>
    /// Получение роута для Post запроса
    /// </summary>
    public string Route { get;}

    /// <summary>
    /// Инициализазация нового экземпляра PostAttribute с полученным роутом
    /// </summary>
    /// <param name="route">Роут по которому находится метод вызываемый Post запросом</param>
    public PostAttribute(string route)
    {
        Route = route;
    }
}