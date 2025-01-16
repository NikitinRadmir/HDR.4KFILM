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

public class MovieRepository
{
    private  ORMContext<Movie> _movieContext = new(new SqlConnection(AppConfig.GetInstance().ConnectionString));

    public List<Movie> GetMovies()
    {
        return _movieContext.ReadAll();
    }

    public Movie GetMovieById(int id)
    {
        return _movieContext.ReadById(id);
    }

    public Movie CreateMovie(Movie movie)
    {
        return _movieContext.Create(movie);
    }

    public void DeleteMovie(int id)
    {
        _movieContext.Delete(id);
    }

    public void UpdateMovie(Movie movie)
    {
        _movieContext.Update(movie);
    }
}
