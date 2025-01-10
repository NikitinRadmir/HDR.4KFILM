using System.Net;
using HttpServerLibrary.Core;
using HttpServerLibrary.Core.HttpResponse;
using HttpServerLibrary.Models;

namespace HttpServerLibrary.Handlers;

/// <summary>
/// Обработчик статических файлов, наследуется от <see cref="Handler"/>
/// обрабатывает запрос на статический файл и отправляет соответсвующий ответ с контентом
/// </summary>
internal sealed class StaticFileHandler : Handler
{
    /// <summary>
    /// Путь до статических файлов, получаемый из конфигураций <see cref="AppConfig"/>
    /// </summary>
    public string _staticDirectoryPath = AppConfig.GetInstance().Path;

    /// <summary>
    /// Метод обработки запроса на статический файл, обрабатывает данные, подготавливает их и отправляет данные в ответе
    /// передает запрос дальше в случае невозможности его обработки
    /// </summary>
    /// <param name="context"></param>
    public override void HandleRequest(HttpRequestContext context)
    {
        bool isGet = context.Request.HttpMethod.Equals("GET", StringComparison.CurrentCultureIgnoreCase);
        string absolutePath = context.Request.Url.AbsolutePath;

        // Проверяем, заканчивается ли путь слешем
        if (absolutePath == "/")
        {
            context.Response.Redirect("/main");
            context.Response.Close();
            return;
        }

        string[] arr = absolutePath.Split(".");
        bool isFile = arr.Length >= 2;

        if (isGet && isFile)
        {
            string filePath = _staticDirectoryPath + absolutePath;
            if (File.Exists(filePath))
            {
                SendContentResponse(context.Response, filePath);
            }
            else
            {
                SendNotFoundResponse(context.Response);
            }
        }
        // Передача запроса дальше по цепи при наличии в ней обработчиков
        else if (Successor != null)
        {
            Successor.HandleRequest(context);
        }
        else
        {
            context.Response.Redirect("/main");
            context.Response.Close();
        }
    }

    /// <summary>
    /// Метод отправки контента в ответе в случае его удачного получения
    /// </summary>
    /// <param name="response">Ответ в который записываются данные</param>
    /// <param name="filePath">Путь до запрашиваемого файла</param>
    private void SendContentResponse(HttpListenerResponse response, string filePath)
    {
        // определение типа контента
        response.ContentType = GetContentType(filePath);

        // Формирование ответа отправляемый в ответ код html возвращает
        byte[] buffer = File.ReadAllBytes(filePath);

        response.ContentLength64 = buffer.Length;
        response.OutputStream.Write(buffer, 0, buffer.Length);

        // Закрываем ответ
        response.OutputStream.Close();
        response.Close();
    }

    /// <summary>
    /// Метод отправки страницы 404.html в случае когда запрашиваемый контент не найден
    /// </summary>
    /// <param name="response"></param>
    private void SendNotFoundResponse(HttpListenerResponse response)
    {
        byte[] buffer = File.ReadAllBytes(_staticDirectoryPath + "/404.html");
        response.StatusCode = 404;
        response.ContentLength64 = buffer.Length;
        response.OutputStream.Write(buffer, 0, buffer.Length);
        response.OutputStream.Close();
        response.Close();
    }

    /// <summary>
    /// Метод для определения типа контента
    /// </summary>
    /// <param name="filePath">Путь до запрашиваемого контента</param>
    /// <returns>Тип контента или стандартное значение "text/txt"</returns>
    private string GetContentType(string filePath)
    {
        var extension = filePath.Split('.').Last();
        return _contentTypes.GetValueOrDefault(extension, "text/txt");
    }

    /// <summary>
    /// Словарь где ключ это расширение файла, а значение это тип контента
    /// </summary>
    private Dictionary<string, string> _contentTypes = new Dictionary<string, string>()
    {
        {"css","text/css"},
        {"html","text/html"},
        {"jpg","image/jpg"},
        {"jpeg","image/jpeg"},
        {"js","text/javascript"},
        {"json","application/json"},
        {"png","image/png"},
        {"svg","image/svg+xml"},
        {"ttf","font/ttf"},
        {"webp","image/webp"},
    };
}
