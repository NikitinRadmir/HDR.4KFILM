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
/// Представляет конечную точку для операций аутентификации.
/// </summary>
public class AuthEndpoint : EndpointBase
{
    /// <summary>
    /// Получает или задает помощник ответа.
    /// </summary>
    public virtual IResponseHelper ResponseHelper { get; set; } = new ResponseHelper();

    /// <summary>
    /// Обрабатывает GET-запрос для страницы входа.
    /// </summary>
    /// <returns>Результат HTTP-ответа.</returns>
    [Get("auth/login")]
    public IHttpResponseResult Get()
    {
        var localPath = "Auth/login.html";
        var responseText = ResponseHelper.GetResponseText(localPath);
        return Html(responseText);
    }

    /// <summary>
    /// Обрабатывает POST-запрос для входа пользователя.
    /// </summary>
    /// <param name="login">Логин пользователя.</param>
    /// <param name="password">Пароль пользователя.</param>
    /// <returns>Результат HTTP-ответа.</returns>
    [Post("auth/login")]
    public IHttpResponseResult Login(string login, string password)
    {
        var localPath = "Auth/login.html";
        var responseText = ResponseHelper.GetResponseText(localPath);
        var templateEngine = new HtmlTemplateEngine();

        Console.WriteLine("----- User on login-page -----");
        Console.WriteLine($"Login attempt with login: {login} and password: {password}");


        // Создание экземпляра UserRepository
        var userRepository = new UserRepository();

        // Получение всех пользователей
        var users = userRepository.GetUsers();

        // Поиск пользователя по логину и паролю
        var user = users.FirstOrDefault(x => x.Login == login && x.Password == password);

        if (user == null)
        {
            Console.WriteLine("User not found in the database.");
            var result = templateEngine.Render(responseText, "<!--ERROR: USER DOESN'T EXIST-->", "<p class=\"error_message\">Такого пользователя не существует</p>");
            return Html(result);
        }

        Console.WriteLine($"User found: {user.Login}, {user.Password}");

        var token = Guid.NewGuid().ToString();
        Cookie nameCookie = new Cookie("session-token", token);
        nameCookie.Path = "/";
        Context.Response.Cookies.Add(nameCookie);
        SessionStorage.SaveSession(token, user.Id.ToString());
        return Redirect("/main");
    }
}
