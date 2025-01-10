using System.Net;
using HttpServerLibrary.Core;

namespace HttpServerLibrary.Handlers;
/// <summary>
/// Абстрактный класс обработчика запросов в цепочке обязанностей
/// </summary>
public abstract class Handler
{
    /// <summary>
    /// Следующий обработчик в цепочке обязанностей
    /// вызывается в случае, когда текущий обработчик не может обработать запрос
    /// </summary>
    public Handler Successor { get; set; }
    /// <summary>
    /// Метод обработки запроса
    /// принимая контекст обрабатываемого запроса реализует логику его обработки
    /// </summary>
    /// <param name="context">Контекст текущего запроса</param>
    public abstract void HandleRequest(HttpRequestContext context);
}