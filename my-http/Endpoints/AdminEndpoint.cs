using System.Data.SqlClient;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using HttpServerLibrary;
using HttpServerLibrary.Core;
using HttpServerLibrary.Core.Attributes;
using HttpServerLibrary.Core.HttpResponse;
using HttpServerLibrary.Models;
using MyHttpServer.Helpers;
using MyHttpServer.Models;
using MyORMLibrary;
using TemlateEngine;

namespace MyHttpServer.Endpoints;

/// <summary>
/// Представляет конечную точку для административных операций.
/// </summary>
public class AdminEndpoint : EndpointBase
{
    /// <summary>
    /// Получает или задает помощник ответа.
    /// </summary>
    public virtual IResponseHelper ResponseHelper { get; set; } = new ResponseHelper();

    /// <summary>
    /// Обрабатывает GET-запрос для страницы входа администратора.
    /// </summary>
    /// <returns>Результат HTTP-ответа.</returns>
    [Get("admlogin")]
    public IHttpResponseResult Get()
    {
        var localPath = "Auth/admin-login.html";
        var responseText = ResponseHelper.GetResponseText(localPath);
        return Html(responseText);
    }

    /// <summary>
    /// Обрабатывает POST-запрос для входа администратора.
    /// </summary>
    /// <param name="login">Логин администратора.</param>
    /// <param name="password">Пароль администратора.</param>
    /// <returns>Результат HTTP-ответа.</returns>
    [Post("admlogin")]
    public IHttpResponseResult Login(string login, string password)
    {
        var localPath = "Auth/admin-login.html";
        var responseText = ResponseHelper.GetResponseText(localPath);
        var templateEngine = new HtmlTemplateEngine();
        var admin_context = new ORMContext<Admin>(new SqlConnection(AppConfig.GetInstance().ConnectionString));

        // Логирование входных данных
        Console.WriteLine("----- User on admin-login-page -----");
        Console.WriteLine($"Login attempt with login: {login} and password: {password}");

        var admin = admin_context.FirstOrDefault(x => x.Login == login && x.Password == password);

        // Логирование результата запроса
        if (admin == null)
        {
            Console.WriteLine("admin not found in the database.");
            var result = templateEngine.Render(responseText, "<!--ERROR: ADMIN DOESN'T EXIST-->", "<p class=\"error_message\">Такого администратора не существует</p>");
            return Html(result);
        }

        Console.WriteLine($"admin found: {admin.Login}, {admin.Password}");

        var token = Guid.NewGuid().ToString();
        Cookie nameCookie = new Cookie("admin-session-token", token);
        nameCookie.Path = "/";
        Context.Response.Cookies.Add(nameCookie);
        SessionStorage.SaveSession(token, admin.Id.ToString());

        Console.WriteLine("admin found and redirecting to index");
        return Redirect("/admin");
    }

    /// <summary>
    /// Обрабатывает GET-запрос для административной страницы.
    /// </summary>
    /// <returns>Результат HTTP-ответа.</returns>
    [Get("admin")]
    public IHttpResponseResult GetPage()
    {
        var localPath = "Admin/admin.html";
        var responseText = ResponseHelper.GetResponseText(localPath);

        Console.WriteLine("----- admin on admin-page -----");

        var movie_context = new ORMContext<Movie>(new SqlConnection(AppConfig.GetInstance().ConnectionString));
        var movies = movie_context.ReadAll("Movies");

        var user_context = new ORMContext<User>(new SqlConnection(AppConfig.GetInstance().ConnectionString));
        var users = user_context.GetAll();

        var admin_context = new ORMContext<Admin>(new SqlConnection(AppConfig.GetInstance().ConnectionString));
        var admins = admin_context.GetAll();

        var moviedata_context = new ORMContext<MovieData>(new SqlConnection(AppConfig.GetInstance().ConnectionString));
        var moviedatas = moviedata_context.ReadAll("MovieDatas");

        var genre_context = new ORMContext<Genre>(new SqlConnection(AppConfig.GetInstance().ConnectionString));
        var genres = genre_context.ReadAll("Genres");

        var country_context = new ORMContext<Country>(new SqlConnection(AppConfig.GetInstance().ConnectionString));
        var countries = country_context.ReadAll("Countrys");

        var templateEngine = new HtmlTemplateEngine();
        var model = new
        {
            movies = movies,
            admins = admins,
            users = users,
            moviedatas = moviedatas,
            genres = genres,
            countries = countries,
        };

        if (!IsAuthorized(Context))
        {
            return Redirect("/admlogin");
        }

        var text = templateEngine.Render(responseText, model);
        return Html(text);
    }

    /// <summary>
    /// Проверяет, авторизован ли пользователь.
    /// </summary>
    /// <param name="context">Контекст HTTP-запроса.</param>
    /// <returns>True, если пользователь авторизован, иначе false.</returns>
    public bool IsAuthorized(HttpRequestContext context)
    {
        // Проверка наличия Cookie с session-token
        if (context.Request.Cookies.Any(c => c.Name == "admin-session-token"))
        {
            var cookie = context.Request.Cookies["admin-session-token"];
            return SessionStorage.ValidateToken(cookie.Value);
        }

        return false;
    }

    /// <summary>
    /// Обрабатывает POST-запрос для добавления пользователя.
    /// </summary>
    /// <param name="addUserLogin">Логин нового пользователя.</param>
    /// <param name="addUserPassword">Пароль нового пользователя.</param>
    /// <returns>Результат HTTP-ответа.</returns>
    [Post("admin/user/add")]
    public IHttpResponseResult AddUser(string addUserLogin, string addUserPassword)
    {
        try
        {
            var user_context = new ORMContext<User>(new SqlConnection(AppConfig.GetInstance().ConnectionString));
            User newUser = new User
            {
                Login = addUserLogin,
                Password = addUserPassword
            };
            user_context.Create(newUser);
            var user = user_context.GetUserByLogin(addUserLogin);
            return Json(user);
        }
        catch (Exception ex)
        {
            // Логирование ошибки
            Console.WriteLine("Error adding user: " + ex.Message);
            return Json(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Обрабатывает POST-запрос для удаления пользователя по идентификатору.
    /// </summary>
    /// <param name="deleteUserId">Идентификатор пользователя для удаления.</param>
    /// <returns>Результат HTTP-ответа.</returns>
    [Post("admin/user/delete")]
    public IHttpResponseResult DeleteUserById(string deleteUserId)
    {
        try
        {
            Console.WriteLine(deleteUserId);
            var user_context = new ORMContext<User>(new SqlConnection(AppConfig.GetInstance().ConnectionString));
            if (deleteUserId != null)
            {
                user_context.Delete(deleteUserId, "Users");
            }
            return Json(user_context.GetAll());
        }
        catch (Exception ex)
        {
            // Логирование ошибки
            Console.WriteLine("Error deleting user: " + ex.Message);
            return Json(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Обрабатывает POST-запрос для добавления фильма.
    /// </summary>
    /// <param name="addTitle">Название фильма.</param>
    /// <param name="addImageUrl">URL изображения фильма.</param>
    /// <param name="addHtmlPageUrl">URL HTML-страницы фильма.</param>
    /// <returns>Результат HTTP-ответа.</returns>
    [Post("admin/movie/add")]
    public IHttpResponseResult AddMovie(string addTitle, string addImageUrl, string addHtmlPageUrl)
    {
        try
        {
            Console.WriteLine($"Adding movie with title: {addTitle}, image URL: {addImageUrl}, HTML page URL: {addHtmlPageUrl}");
            var movie_context = new ORMContext<Movie>(new SqlConnection(AppConfig.GetInstance().ConnectionString));

            Movie newMovie = new Movie
            {
                Title = addTitle,
                ImageUrl = addImageUrl,
                HtmlPageUrl = addHtmlPageUrl,
            };
            Console.WriteLine("Attempting to add movie to the database...");
            movie_context.CreateMovie(newMovie);
            Console.WriteLine("Movie added successfully.");

            var movie = movie_context.GetByTitle(addTitle);
            return Json(movie);
        }
        catch (Exception ex)
        {
            // Логирование ошибки
            Console.WriteLine("Error adding movie: " + ex.Message);
            return Json(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Обрабатывает POST-запрос для удаления фильма по идентификатору.
    /// </summary>
    /// <param name="deleteMovieId">Идентификатор фильма для удаления.</param>
    /// <returns>Результат HTTP-ответа.</returns>
    [Post("admin/movie/delete")]
    public IHttpResponseResult DeleteMovieById(string deleteMovieId)
    {
        try
        {
            Console.WriteLine(deleteMovieId);
            var movie_context = new ORMContext<Movie>(new SqlConnection(AppConfig.GetInstance().ConnectionString));
            if (deleteMovieId != null)
            {
                movie_context.DeleteMovie(deleteMovieId, "Movies");
            }
            return Json(movie_context.GetAll());
        }
        catch (Exception ex)
        {
            // Логирование ошибки
            Console.WriteLine("Error deleting movie: " + ex.Message);
            return Json(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Обрабатывает POST-запрос для добавления данных фильма.
    /// </summary>
    /// <param name="addMovieDataTitle">Название фильма.</param>
    /// <param name="addMovieDataCoverImageUrl">URL обложки фильма.</param>
    /// <param name="addMovieDataDescription">Описание фильма.</param>
    /// <param name="addMovieDataOriginalTitle">Оригинальное название фильма.</param>
    /// <param name="addMovieDataYear">Год выпуска фильма.</param>
    /// <param name="addMovieDataCountry">Страна фильма.</param>
    /// <param name="addMovieDataGenre">Жанр фильма.</param>
    /// <param name="addMovieDataQuality">Качество фильма.</param>
    /// <param name="addMovieDataSound">Звук фильма.</param>
    /// <param name="addMovieDataDirector">Режиссер фильма.</param>
    /// <param name="addMovieDataCast">Актеры фильма.</param>
    /// <param name="addMovieDataMoviePlayer">Плеер фильма.</param>
    /// <returns>Результат HTTP-ответа.</returns>
    [Post("admin/moviedata/add")]
    public IHttpResponseResult AddMovieData(string addMovieDataTitle, string addMovieDataCoverImageUrl, string addMovieDataDescription, string addMovieDataOriginalTitle, int addMovieDataYear, string addMovieDataCountry, string addMovieDataGenre, string addMovieDataQuality, string addMovieDataSound, string addMovieDataDirector, string addMovieDataCast, string addMovieDataMoviePlayer)
    {
        try
        {
            var moviedata_context = new ORMContext<MovieData>(new SqlConnection(AppConfig.GetInstance().ConnectionString));
            MovieData newMovieData = new MovieData
            {
                Title = addMovieDataTitle,
                CoverImageUrl = addMovieDataCoverImageUrl,
                Description = addMovieDataDescription,
                OriginalTitle = addMovieDataOriginalTitle,
                Year = addMovieDataYear,
                Country = addMovieDataCountry,
                Genre = addMovieDataGenre,
                Quality = addMovieDataQuality,
                Sound = addMovieDataSound,
                Director = addMovieDataDirector,
                Cast = addMovieDataCast,
                MoviePlayer = addMovieDataMoviePlayer
            };
            moviedata_context.CreateMovieData(newMovieData);
            var movieData = moviedata_context.GetByTitle(addMovieDataTitle);
            return Json(movieData);
        }
        catch (Exception ex)
        {
            // Логирование ошибки
            Console.WriteLine("Error adding MovieData: " + ex.Message);
            return Json(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Обрабатывает POST-запрос для удаления данных фильма по идентификатору.
    /// </summary>
    /// <param name="deleteMovieDataId">Идентификатор данных фильма для удаления.</param>
    /// <returns>Результат HTTP-ответа.</returns>
    [Post("admin/moviedata/delete")]
    public IHttpResponseResult DeleteMovieDataById(string deleteMovieDataId)
    {
        try
        {
            Console.WriteLine(deleteMovieDataId);
            var moviedata_context = new ORMContext<MovieData>(new SqlConnection(AppConfig.GetInstance().ConnectionString));
            if (deleteMovieDataId != null)
            {
                moviedata_context.DeleteMovieData(deleteMovieDataId, "MovieDatas");
            }
            return Json(moviedata_context.GetAll());
        }
        catch (Exception ex)
        {
            // Логирование ошибки
            Console.WriteLine("Error deleting MovieData: " + ex.Message);
            return Json(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Обрабатывает POST-запрос для добавления жанра.
    /// </summary>
    /// <param name="addGenreName">Название жанра.</param>
    /// <returns>Результат HTTP-ответа.</returns>
    [Post("admin/genre/add")]
    public IHttpResponseResult AddGenre(string addGenreName)
    {
        try
        {
            var genre_context = new ORMContext<Genre>(new SqlConnection(AppConfig.GetInstance().ConnectionString));
            Genre newGenre = new Genre
            {
                GenreName = addGenreName
            };
            genre_context.Create(newGenre);
            var genre = genre_context.GetByGenreName(addGenreName);
            return Json(genre);
        }
        catch (Exception ex)
        {
            // Логирование ошибки
            Console.WriteLine("Error adding Genre: " + ex.Message);
            return Json(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Обрабатывает POST-запрос для удаления жанра по идентификатору.
    /// </summary>
    /// <param name="deleteGenreId">Идентификатор жанра для удаления.</param>
    /// <returns>Результат HTTP-ответа.</returns>
    [Post("admin/genre/delete")]
    public IHttpResponseResult DeleteGenreById(string deleteGenreId)
    {
        try
        {
            Console.WriteLine(deleteGenreId);
            var genre_context = new ORMContext<Genre>(new SqlConnection(AppConfig.GetInstance().ConnectionString));
            if (deleteGenreId != null)
            {
                genre_context.Delete(deleteGenreId, "Genres");
            }
            return Json(genre_context.GetAll());
        }
        catch (Exception ex)
        {
            // Логирование ошибки
            Console.WriteLine("Error deleting Genre: " + ex.Message);
            return Json(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Обрабатывает POST-запрос для добавления страны.
    /// </summary>
    /// <param name="addCountryName">Название страны.</param>
    /// <returns>Результат HTTP-ответа.</returns>
    [Post("admin/country/add")]
    public IHttpResponseResult AddCountry(string addCountryName)
    {
        try
        {
            var country_context = new ORMContext<Country>(new SqlConnection(AppConfig.GetInstance().ConnectionString));
            Country newCountry = new Country
            {
                CountryName = addCountryName
            };
            country_context.Create(newCountry);
            var country = country_context.GetByCountryName(addCountryName);
            return Json(country);
        }
        catch (Exception ex)
        {
            // Логирование ошибки
            Console.WriteLine("Error adding Country: " + ex.Message);
            return Json(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Обрабатывает POST-запрос для удаления страны по идентификатору.
    /// </summary>
    /// <param name="deleteCountryId">Идентификатор страны для удаления.</param>
    /// <returns>Результат HTTP-ответа.</returns>
    [Post("admin/country/delete")]
    public IHttpResponseResult DeleteCountryById(string deleteCountryId)
    {
        try
        {
            Console.WriteLine(deleteCountryId);
            var country_context = new ORMContext<Country>(new SqlConnection(AppConfig.GetInstance().ConnectionString));
            if (deleteCountryId != null)
            {
                country_context.Delete(deleteCountryId, "Countrys");
            }
            return Json(country_context.GetAll());
        }
        catch (Exception ex)
        {
            // Логирование ошибки
            Console.WriteLine("Error deleting Country: " + ex.Message);
            return Json(new { error = ex.Message });
        }
    }
}
