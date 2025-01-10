using System.Data.Common;
using System.Data.SqlClient;
using HttpServerLibrary;
using HttpServerLibrary.Core;
using HttpServerLibrary.Core.Attributes;
using HttpServerLibrary.Core.HttpResponse;
using HttpServerLibrary.Models;
using MyHttpServer.Models;
using MyORMLibrary;

namespace MyServer.Endpoints;

/// <summary>
/// ������������ �������� ����� ��� �������� � ��������������.
/// </summary>
public class UserEndpoints : EndpointBase
{
    /// <summary>
    /// ������������ GET-������ ��� ��������� ������������ �� ��������������.
    /// </summary>
    /// <param name="id">������������� ������������.</param>
    /// <returns>��������� HTTP-������ � ������� JSON.</returns>
    [Get("user")]
    public IHttpResponseResult GetUserById(int id)
    {
        var context = new ORMContext<User>(new SqlConnection(AppConfig.GetInstance().ConnectionString));

        var user = context.ReadById(id, "Users");
        return Json(user);
    }

    /// <summary>
    /// ������������ GET-������ ��� ��������� ���� �������������.
    /// </summary>
    /// <returns>��������� HTTP-������ � ������� JSON.</returns>
    [Get("users")]
    public IHttpResponseResult GetAllUsers()
    {
        var context = new ORMContext<User>(new SqlConnection(AppConfig.GetInstance().ConnectionString));

        var user = context.Where(u => u.Id == 3 || u.Id == 2);
        return Json(user);
    }

    /// <summary>
    /// ������������ GET-������ ��� �������� ������������ �� ��������������.
    /// </summary>
    /// <param name="id">������������� ������������.</param>
    /// <returns>��������� HTTP-������ � ������� JSON.</returns>
    [Get("deleteuser")]
    public IHttpResponseResult DeleteUser(int id)
    {
        var context = new ORMContext<User>(new SqlConnection(AppConfig.GetInstance().ConnectionString));

        var user = context.Delete(id, "Users");
        return Json(user);
    }
}
