using System.Data.Common;
using System.Data.SqlClient;
using HttpServerLibrary;
using HttpServerLibrary.Core;
using HttpServerLibrary.Core.Attributes;
using HttpServerLibrary.Core.HttpResponse;
using HttpServerLibrary.Models;
using MyHttpServer.Helpers;
using MyHttpServer.Models;
using MyHttpServer.Repositories;
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
    [Get("film")]
    public IHttpResponseResult GetPage(int id)
    {
        Console.WriteLine("----- Movie page request -----");

        var localPath = "Movies/movie.html";
        var responseText = ResponseHelper.GetResponseText(localPath);
        Console.WriteLine($"Loaded response text from: {localPath}");

        //// Извлечение данных о фильмах
        var movieRepository = new MovieDataRepository();

        var movieData = movieRepository.GetMovieDataById(id);

        if (movieData == null)
        {
            Console.WriteLine($"Movie with id {id} not found.");
            return Html("<!DOCTYPE html>\r\n<html lang=\"ru\">\r\n<head>\r\n    <meta charset=\"utf-8\">\r\n    <title>hdr.4kfilm Страница не найдена - 404</title>\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <style>\r\n        body {\r\n            font-family: Arial, sans-serif;\r\n            background-color: #f4f4f4;\r\n            color: #333;\r\n            text-align: center;\r\n            padding: 50px;\r\n        }\r\n        .container {\r\n            background: #fff;\r\n            padding: 20px;\r\n            border-radius: 8px;\r\n            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);\r\n            display: inline-block;\r\n            text-align: center;\r\n        }\r\n        h1 {\r\n            font-size: 3em;\r\n            margin-bottom: 0.5em;\r\n        }\r\n        p {\r\n            font-size: 1.2em;\r\n            margin-bottom: 1em;\r\n        }\r\n        a {\r\n            color: #007BFF;\r\n            text-decoration: none;\r\n            font-size: 1.2em;\r\n        }\r\n        a:hover {\r\n            text-decoration: underline;\r\n        }\r\n    </style>\r\n</head>\r\n<body>\r\n    <div class=\"container\">\r\n        <h1>404</h1>\r\n        <p>Страница не найдена</p>\r\n        <p>Извините, но запрашиваемая вами страница не существует.</p>\r\n\r\n    </div>\r\n</body>\r\n</html>\r\n");
        }
        Console.WriteLine($"Movie data loaded: {movieData}");

        var templateEngine = new HtmlTemplateEngine();
        var text = templateEngine.Render(responseText, movieData);
        Console.WriteLine("Rendered template with movie data.");

        return Html(text);
    }
}
