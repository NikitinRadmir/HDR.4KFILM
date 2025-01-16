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

public class MovieDataRepository
{
    private ORMContext<MovieData> _movieDataContext = new(new SqlConnection(AppConfig.GetInstance().ConnectionString));

    public List<MovieData> GetMovieData()
    {
        return _movieDataContext.ReadAll();
    }

    public MovieData GetMovieDataById(int id)
    {
        return _movieDataContext.ReadById(id);
    }

    public MovieData CreateMovieData(MovieData movieData)
    {
        return _movieDataContext.CreateMovieData(movieData);
    }

    public void DeleteMovieData(int id)
    {
        _movieDataContext.Delete(id);
    }

    public void UpdateMovieData(MovieData movieData)
    {
        _movieDataContext.Update(movieData);
    }
}
