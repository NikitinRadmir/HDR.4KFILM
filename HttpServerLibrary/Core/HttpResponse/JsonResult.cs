using System.Text;
using System.Text.Json;

namespace HttpServerLibrary.Core.HttpResponse;
/// <summary>
/// Класс реализующий <see cref="IHttpResponseResult"/>
/// Подготавиливает Json файл и заливает его в Response
/// </summary>
public class JsonResult : IHttpResponseResult
{
    /// <summary>
    /// Данные для заполнения Json файла
    /// </summary>
    private readonly object _data;

    /// <summary>
    /// Получение приватного поля _data
    /// Нужно для тестировния приложения
    /// </summary>
    /// <returns><see cref="_data"/></returns>
    public object GetData()
    {
        return _data;
    }

    /// <summary>
    /// Инициализирует instance of <see cref="JsonResult"/>
    /// </summary>
    /// <param name="data">данные Json файла для отправки в ответе</param>
    public JsonResult(object data)
    {
        _data = data;
    }
    
    public void Execute(HttpRequestContext context)
    {
        var json = JsonSerializer.Serialize(_data);
        var buffer = Encoding.UTF8.GetBytes(json);
        context.Response.ContentLength64 = buffer.Length;
        context.Response.ContentType = "application/json";
        using Stream output = context.Response.OutputStream;
        output.Write(buffer);
        output.Flush();
    }
}