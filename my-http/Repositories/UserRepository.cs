using HttpServerLibrary.Models;
using MyHttpServer.Models;
using MyORMLibrary;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHttpServer.Repositories;

public class UserRepository
{
    private ORMContext<User> _userContext = new(new SqlConnection(AppConfig.GetInstance().ConnectionString));

    public List<User> GetUsers()
    {
        return _userContext.ReadAll();
    }
    public User GetUserById(int id)
    {
        return _userContext.ReadById(id);
    }

    public User CreateUser(User user)
    {
        return _userContext.Create(user);
    }

    public void DeleteUser(int id)
    {
        _userContext.Delete(id);
    }
    public void UpdateUser(User user)
    {
        _userContext.Update(user);
    }
}
