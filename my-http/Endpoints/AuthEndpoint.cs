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
/// ������������ �������� ����� ��� �������� ��������������.
/// </summary>
public class AuthEndpoint : EndpointBase
{
    /// <summary>
    /// �������� ��� ������ �������� ������.
    /// </summary>
    public virtual IResponseHelper ResponseHelper { get; set; } = new ResponseHelper();

    /// <summary>
    /// ������������ GET-������ ��� �������� �����.
    /// </summary>
    /// <returns>��������� HTTP-������.</returns>
    [Get("auth/login")]
    public IHttpResponseResult Get()
    {
        var localPath = "Auth/login.html";
        var responseText = ResponseHelper.GetResponseText(localPath);
        return Html(responseText);
    }

    /// <summary>
    /// ������������ POST-������ ��� ����� ������������.
    /// </summary>
    /// <param name="login">����� ������������.</param>
    /// <param name="password">������ ������������.</param>
    /// <returns>��������� HTTP-������.</returns>
    [Post("auth/login")]
    public IHttpResponseResult Login(string login, string password)
    {
        var localPath = "Auth/login.html";
        var responseText = ResponseHelper.GetResponseText(localPath);
        var templateEngine = new HtmlTemplateEngine();

        Console.WriteLine("----- User on login-page -----");
        Console.WriteLine($"Login attempt with login: {login} and password: {password}");


        // �������� ���������� UserRepository
        var userRepository = new UserRepository();

        // ��������� ���� �������������
        var users = userRepository.GetUsers();

        // ����� ������������ �� ������ � ������
        var user = users.FirstOrDefault(x => x.Login == login && x.Password == password);

        if (user == null)
        {
            Console.WriteLine("User not found in the database.");
            var result = templateEngine.Render(responseText, "<!--ERROR: USER DOESN'T EXIST-->", "<p class=\"error_message\">������ ������������ �� ����������</p>");
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
