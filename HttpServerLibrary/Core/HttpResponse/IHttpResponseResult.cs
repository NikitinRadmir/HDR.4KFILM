using System.Net;

namespace HttpServerLibrary.Core.HttpResponse;
/// <summary>
/// Интерфейс для классов генерирующих HTTP ответ
/// </summary>
public interface IHttpResponseResult
{
    /// <summary>
    /// Метод отправки и преобразования данных к контекст запроса
    /// </summary>
    /// <param name="context"></param>
    void Execute(HttpRequestContext context);
}