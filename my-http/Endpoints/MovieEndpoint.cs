using System.Data.Common;
using System.Data.SqlClient;
using HttpServerLibrary;
using HttpServerLibrary.Core;
using HttpServerLibrary.Core.Attributes;
using HttpServerLibrary.Core.HttpResponse;
using HttpServerLibrary.Models;
using MyHttpServer.Helpers;
using MyHttpServer.Models;
using MyORMLibrary;
using TemlateEngine;

namespace MyHttpServer.Endpoints;

/// <summary>
/// Представляет конечную точку для операций с фильмами.
/// </summary>
public class MovieEndpoint : EndpointBase
{
    /// <summary>
    /// Получает или задает помощник ответа.
    /// </summary>
    public virtual IResponseHelper ResponseHelper { get; set; } = new ResponseHelper();

    /// <summary>
    /// Обрабатывает GET-запрос для отображения страницы фильма.
    /// </summary>
    /// <returns>Результат HTTP-ответа.</returns>
    [Get("dear-santa")]
    public IHttpResponseResult GetPage()
    {
        Console.WriteLine("----- Movie page request -----");

        var localPath = "Movies/dear-santa.html";
        var responseText = ResponseHelper.GetResponseText(localPath);
        Console.WriteLine($"Loaded response text from: {localPath}");

        // Извлечение данных о фильмах
        var movieContext = new ORMContext<MovieData>(new SqlConnection(AppConfig.GetInstance().ConnectionString));
        var movies = movieContext.ReadMovieById(1);
        Console.WriteLine($"Movie data loaded: {movies}");

        var templateEngine = new HtmlTemplateEngine();
        var text = templateEngine.Render(responseText, movies);
        Console.WriteLine("Rendered template with movie data.");

        return Html(text);
    }
}
