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

public class AdminRepository
{
    private ORMContext<Admin> _adminContext = new(new SqlConnection(AppConfig.GetInstance().ConnectionString));

    public List<Admin> GetAdmins()
    {
        return _adminContext.ReadAll();
    }
}