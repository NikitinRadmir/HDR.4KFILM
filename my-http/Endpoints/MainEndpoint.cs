using System.Data.SqlClient;
using System.Net;
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
/// Представляет основную конечную точку для отображения главной страницы.
/// </summary>
public class MainEndpoint : EndpointBase
{
    /// <summary>
    /// Получает или задает помощник ответа.
    /// </summary>
    public virtual IResponseHelper ResponseHelper { get; set; } = new ResponseHelper();

    /// <summary>
    /// Обрабатывает GET-запрос для отображения главной страницы.
    /// </summary>
    /// <returns>Результат HTTP-ответа.</returns>
    [Get("main")]
    public IHttpResponseResult GetPage()
    {
        Console.WriteLine("----- Main page request -----");

        var localPath = "Main/index.html";
        var responseText = ResponseHelper.GetResponseText(localPath);
        var templateEngine = new HtmlTemplateEngine();

        var movieRepository = new MovieRepository();
        var genreRepository = new GenreRepository();
        var countryRepository = new CountryRepository();

        // Извлечение данных о фильмах
        var movies = movieRepository.GetMovies();
        Console.WriteLine($"Movies loaded: {movies.Count}");

        // Извлечение данных о жанрах
        var genres = genreRepository.GetGenres();
        Console.WriteLine($"Genres loaded: {genres.Count}");

        // Извлечение данных о странах
        var countries = countryRepository.GetCountries();
        Console.WriteLine($"Countries loaded: {countries.Count}");

        dynamic model = new
        {
            Movies = movies,
            Genres = genres,
            Countries = countries
        };

        if (IsAuthorized(Context)) // Используем метод проверки авторизации
        {
            var userId = int.Parse(SessionStorage.GetUserId(Context.Request.Cookies["session-token"].Value));
            var userRepository = new UserRepository();
            var user = userRepository.GetUserById(Convert.ToInt32(userId));
            Console.WriteLine($"User loaded: {user.Login}");

            // Добавление данных пользователя в модель
            model = new
            {
                User = user,
                Movies = movies,
                Genres = genres,
                Countries = countries
            };
        }

        var text = templateEngine.Render(responseText, model);
        return Html(text);
    }

    /// <summary>
    /// Обрабатывает GET-запрос для проверки авторизации пользователя.
    /// </summary>
    /// <returns>Результат HTTP-ответа.</returns>
    [Get("auth/check")]
    public IHttpResponseResult CheckAuthorization()
    {
        if (!IsAuthorized(Context))
        {
            return Json(new { isAuthorized = false });
        }

        var userId = Int32.Parse(SessionStorage.GetUserId(Context.Request.Cookies["session-token"].Value));
        var userRepository = new UserRepository();
        var user = userRepository.GetUserById(Convert.ToInt32(userId));

        return Json(new { isAuthorized = true, username = user.Login });
    }

    /// <summary>
    /// Проверяет, авторизован ли пользователь.
    /// </summary>
    /// <param name="context">Контекст HTTP-запроса.</param>
    /// <returns>True, если пользователь авторизован, иначе false.</returns>
    public bool IsAuthorized(HttpRequestContext context)
    {
        // Проверка наличия Cookie с session-token
        if (context.Request.Cookies.Any(c => c.Name == "session-token"))
        {
            var cookie = context.Request.Cookies["session-token"];
            return SessionStorage.ValidateToken(cookie.Value);
        }

        return false;
    }
}
