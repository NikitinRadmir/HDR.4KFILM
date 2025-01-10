using System.Net;
using System.Text;

namespace HttpServerLibrary.Core.HttpResponse;
/// <summary>
/// Класс реализующий <see cref="IHttpResponseResult"/>
/// Подготавливает HTML контент для отправки ответа и заливает его в Response
/// </summary>
public class HtmlResult : IHttpResponseResult
{
    /// <summary>
    /// текст HTML документа
    /// </summary>
    private readonly string _html;

    /// <summary>
    /// Получение приватного поля _html
    /// Нужно для тестировния приложения
    /// </summary>
    /// <returns><see cref="_html"/></returns>
    public string GetHtml()
    {
        return _html;
    }
    
    /// <summary>
    /// Инициализирует instance of <see cref="HtmlResult"/>
    /// </summary>
    /// <param name="html">текст HTML документа для отправки в ответе</param>
    public HtmlResult(string html)
    {
        _html = html;
    }

    /// <summary>
    /// Оправляет HTML документ в контексте текущего запроса
    /// </summary>
    /// <param name="context">Контекст текущего запроса</param>
    public void Execute(HttpRequestContext context)
    {
        //преобразуем HTML текст в байты для отправки в потом ответа
        byte[] buffer = Encoding.UTF8.GetBytes(_html);

        // получаем поток ответа и пишем в него ответ
        context.Response.ContentLength64 = buffer.Length;
        using Stream output = context.Response.OutputStream;

        // отправляем данные
        output.Write(buffer);
        //сбрасываем данные из буфера
        output.Flush();
    }
}