using System.Net;
using System.Text;
using HttpServerLibrary.Core;
using HttpServerLibrary.Models;

namespace HttpServerLibrary.Handlers;
/// <summary>
/// Класс обработчик ситуации когда не найден метод для обработки запроса по запрашиваемому роутингу
/// </summary>
internal class NotFoundHandler : Handler
{
    /// <summary>
    /// Путь до директории хранения статических файлов
    /// </summary>
    private static string _staticDirectoryPath = AppConfig.GetInstance().Path;
    /// <summary>
    /// Метод обработки запроса, принимая контекст записывает в ответ стрницу 404.html или сообщение о том что страница не найдена
    /// </summary>
    /// <param name="context"><see cref="HttpRequestContext"/> запроса</param>
    public override void HandleRequest(HttpRequestContext context)
    {
        if (File.Exists(_staticDirectoryPath + "404.html"))
        {
            byte[] buffer = Encoding.UTF8.GetBytes(File.ReadAllText(_staticDirectoryPath + "/404.html"));
            SendNotFoundResponse(buffer, context.Response);
        }
        else
        {
            byte[] buffer = Encoding.UTF8.GetBytes("404 NOT FOUND");
            SendNotFoundResponse(buffer, context.Response);
        }
    }
    /// <summary>
    /// Метод для подготовки текста ответа
    /// </summary>
    /// <param name="buffer">байтовое представление контента</param>
    /// <param name="response">Ответ для записи контента</param>
    private void SendNotFoundResponse(byte[] buffer, HttpListenerResponse response)
    {
        response.StatusCode = 404;
        response.ContentLength64 = buffer.Length;
        response.OutputStream.Write(buffer, 0, buffer.Length);
        response.OutputStream.Close();
    }
}