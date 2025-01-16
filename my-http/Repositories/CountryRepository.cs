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

public class CountryRepository
{
    private ORMContext<Country> _countryContext = new(new SqlConnection(AppConfig.GetInstance().ConnectionString));

    public List<Country> GetCountries()
    {
        return _countryContext.ReadAll();
    }
    public Country GetCountryById(int id)
    {
        return _countryContext.ReadById(id);
    }

    public Country CreateCountry(Country country)
    {
        return _countryContext.Create(country);
    }

    public void DeleteCountry(int id)
    {
        _countryContext.Delete(id);
    }
    public void UpdateCountry(Country country)
    {
        _countryContext.Update(country);
    }
}
