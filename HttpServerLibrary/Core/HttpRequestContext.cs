using System.Net;

namespace HttpServerLibrary.Core;
/// <summary>
/// Кастомный контекст http запроса
/// Объединяет Response и Request
/// Для удобства написания кода
/// </summary>
public class HttpRequestContext
{
    /// <summary>
    /// Request <see cref="HttpListenerRequest"/>
    /// Содержит данные входящего запроса
    /// </summary>
    public HttpListenerRequest Request { get; }
    /// <summary>
    /// Response <see cref="HttpListenerResponse"/>
    /// Содержит данные для формирования ответа
    /// </summary>
    public HttpListenerResponse Response { get; }

    /// <summary>
    /// Конструктор создвет instance <see cref="HttpRequestContext"/>
    /// инициализирует поля <see cref="Request"/>
    /// <see cref="Response"/>
    /// </summary>
    /// <param name="request"><see cref="Request"/></param>
    /// <param name="response"><see cref="Response"/></param>
    public HttpRequestContext(HttpListenerRequest request, HttpListenerResponse response)
    {
        Response = response;
        Request = request;
    }
}