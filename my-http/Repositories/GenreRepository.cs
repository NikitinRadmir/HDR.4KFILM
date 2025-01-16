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

public class GenreRepository
{
    private ORMContext<Genre> _genreContext = new(new SqlConnection(AppConfig.GetInstance().ConnectionString));

    public List<Genre> GetGenres()
    {
        return _genreContext.ReadAll();
    }
    public Genre GetGenreById(int id)
    {
        return _genreContext.ReadById(id);
    }

    public Genre CreateGenre(Genre genre)
    {
        return _genreContext.Create(genre);
    }

    public void DeleteGenre(int id)
    {
        _genreContext.Delete(id);
    }
    public void UpdateGenre(Genre genre)
    {
        _genreContext.Update(genre);
    }
}
