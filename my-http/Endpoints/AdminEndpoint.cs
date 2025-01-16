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
using MyHttpServer.Repositories;
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
        var adminRepository = new AdminRepository();
        var admins = adminRepository.GetAdmins();

        // Логирование входных данных
        Console.WriteLine("----- User on admin-login-page -----");
        Console.WriteLine($"Login attempt with login: {login} and password: {password}");

        var admin = admins.FirstOrDefault(x => x.Login == login && x.Password == password);

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

        var movieRepository = new MovieRepository();
        var movieDataRepository = new MovieDataRepository();
        var userRepository = new UserRepository();
        var adminRepository = new AdminRepository();
        var genreRepository = new GenreRepository();
        var countryRepository = new CountryRepository();

        var movies = movieRepository.GetMovies();

        var users = userRepository.GetUsers();

        var admins = adminRepository.GetAdmins();

        var moviedatas = movieDataRepository.GetMovieData();

        var genres = genreRepository.GetGenres();

        var countries = countryRepository.GetCountries();

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
            var userRepository = new UserRepository();
            User newUser = new User
            {
                Login = addUserLogin,
                Password = addUserPassword
            };
            userRepository.CreateUser(newUser);
            var user = userRepository.GetUserById(newUser.Id);
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
            var userRepository = new UserRepository();
            if (deleteUserId != null)
            {
                userRepository.DeleteUser(Convert.ToInt32(deleteUserId));
            }
            return Json(userRepository.GetUsers());
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
    public IHttpResponseResult AddMovie(string addTitle, string addImageUrl)
    {
        try
        {
            Console.WriteLine($"Adding movie with title: {addTitle}, image URL: {addImageUrl}");
            var movieRepository = new MovieRepository();

            Movie newMovie = new Movie
            {
                Title = addTitle,
                ImageUrl = addImageUrl
            };
            Console.WriteLine("Attempting to add movie to the database...");
            movieRepository.CreateMovie(newMovie);
            Console.WriteLine("Movie added successfully.");

            var movie = movieRepository.GetMovieById(newMovie.MovieId);
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
            var movieRepository = new MovieRepository();
            if (deleteMovieId != null)
            {
                movieRepository.DeleteMovie(Convert.ToInt32(deleteMovieId));
            }
            return Json(movieRepository.GetMovies());
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
    public IHttpResponseResult AddMovieData(int addMovieDataId, string addMovieDataTitle, string addMovieDataCoverImageUrl, string addMovieDataDescription, string addMovieDataOriginalTitle, int addMovieDataYear, string addMovieDataCountry, string addMovieDataGenre, string addMovieDataQuality, string addMovieDataSound, string addMovieDataDirector, string addMovieDataCast, string addMovieDataMoviePlayer)
    {
        try
        {
            var movieDataRepository = new MovieDataRepository();
            MovieData newMovieData = new MovieData
            {
                MovieId = addMovieDataId, // Убедитесь, что значение для MovieId передается
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
            movieDataRepository.CreateMovieData(newMovieData);
            var movieData = movieDataRepository.GetMovieDataById(newMovieData.MovieId);
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
            var movieDataRepository = new MovieDataRepository();
            if (deleteMovieDataId != null)
            {
                movieDataRepository.DeleteMovieData(Convert.ToInt32(deleteMovieDataId));
            }
            return Json(movieDataRepository.GetMovieData());
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
            var genreRepository = new GenreRepository();
            Genre newGenre = new Genre
            {
                GenreName = addGenreName
            };
            genreRepository.CreateGenre(newGenre);
            var genre = genreRepository.GetGenreById(newGenre.Id);
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
            var genreRepository = new GenreRepository();
            if (deleteGenreId != null)
            {
                genreRepository.DeleteGenre(Convert.ToInt32(deleteGenreId));
            }
            return Json(genreRepository.GetGenres());
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
            var countryRepository = new CountryRepository();
            Country newCountry = new Country
            {
                CountryName = addCountryName
            };
            countryRepository.CreateCountry(newCountry);
            var country = countryRepository.GetCountryById(newCountry.Id);
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
            var countryRepository = new CountryRepository();
            if (deleteCountryId != null)
            {
                countryRepository.DeleteCountry(Convert.ToInt32(deleteCountryId));
            }
            return Json(countryRepository.GetCountries());
        }
        catch (Exception ex)
        {
            // Логирование ошибки
            Console.WriteLine("Error deleting Country: " + ex.Message);
            return Json(new { error = ex.Message });
        }
    }

    [Post("admin/user/update")]
    public IHttpResponseResult UpdateUser(int updateUserId, string updateUserLogin, string updateUserPassword)
    {
        try
        {
            var userRepository = new UserRepository();
            var existingUser = userRepository.GetUserById(updateUserId);
            if (existingUser == null)
            {
                return Json(new { error = "User not found" });
            }

            existingUser.Login = updateUserLogin;
            existingUser.Password = updateUserPassword;
            userRepository.UpdateUser(existingUser);
            return Json(userRepository.GetUsers());
        }
        catch (Exception ex)
        {
            // Логирование ошибки
            Console.WriteLine("Error updating user: " + ex.Message);
            return Json(new { error = ex.Message });
        }
    }

    [Post("admin/movie/update")]
    public IHttpResponseResult UpdateMovie(int updateMovieId, string updateTitle, string updateImageUrl)
    {
        try
        {
            var movieRepository = new MovieRepository();
            var existingMovie = movieRepository.GetMovieById(Convert.ToInt32(updateMovieId));
            if (existingMovie == null)
            {
                return Json(new { error = "Movie not found" });
            }

            existingMovie.Title = updateTitle;
            existingMovie.ImageUrl = updateImageUrl;
            movieRepository.UpdateMovie(existingMovie);
            return Json(movieRepository.GetMovies());
        }
        catch (Exception ex)
        {
            // Логирование ошибки
            Console.WriteLine("Error updating movie: " + ex.Message);
            return Json(new { error = ex.Message });
        }
    }


    [Post("admin/moviedata/update")]
    public IHttpResponseResult UpdateMovieData(int updateMovieDataId, string updateMovieDataTitle, string updateMovieDataCoverImageUrl, string updateMovieDataDescription, string updateMovieDataOriginalTitle, int updateMovieDataYear, string updateMovieDataCountry, string updateMovieDataGenre, string updateMovieDataQuality, string updateMovieDataSound, string updateMovieDataDirector, string updateMovieDataCast, string updateMovieDataMoviePlayer)
    {
        try
        {
            var movieDataRepository = new MovieDataRepository();
            var existingMovieData = movieDataRepository.GetMovieDataById(updateMovieDataId);
            if (existingMovieData == null)
            {
                return Json(new { error = "MovieData not found" });
            }

            existingMovieData.Title = updateMovieDataTitle;
            existingMovieData.CoverImageUrl = updateMovieDataCoverImageUrl;
            existingMovieData.Description = updateMovieDataDescription;
            existingMovieData.OriginalTitle = updateMovieDataOriginalTitle;
            existingMovieData.Year = updateMovieDataYear;
            existingMovieData.Country = updateMovieDataCountry;
            existingMovieData.Genre = updateMovieDataGenre;
            existingMovieData.Quality = updateMovieDataQuality;
            existingMovieData.Sound = updateMovieDataSound;
            existingMovieData.Director = updateMovieDataDirector;
            existingMovieData.Cast = updateMovieDataCast;
            existingMovieData.MoviePlayer = updateMovieDataMoviePlayer;
            movieDataRepository.UpdateMovieData(existingMovieData);
            return Json(movieDataRepository.GetMovieData());
        }
        catch (Exception ex)
        {
            // Логирование ошибки
            Console.WriteLine("Error updating MovieData: " + ex.Message);
            return Json(new { error = ex.Message });
        }
    }

    [Post("admin/genre/update")]
    public IHttpResponseResult UpdateGenre(int updateGenreId, string updateGenreName)
    {
        try
        {
            var genreRepository = new GenreRepository();
            var existingGenre = genreRepository.GetGenreById(updateGenreId);
            if (existingGenre == null)
            {
                return Json(new { error = "Genre not found" });
            }

            existingGenre.GenreName = updateGenreName;
            genreRepository.UpdateGenre(existingGenre);
            return Json(genreRepository.GetGenres());
        }
        catch (Exception ex)
        {
            // Логирование ошибки
            Console.WriteLine("Error updating Genre: " + ex.Message);
            return Json(new { error = ex.Message });
        }
    }

    [Post("admin/country/update")]
    public IHttpResponseResult UpdateCountry(int updateCountryId, string updateCountryName)
    {
        try
        {
            var countryRepository = new CountryRepository();
            Country updatedCountry = new Country
            {
                Id = updateCountryId,
                CountryName = updateCountryName
            };
            countryRepository.UpdateCountry(updatedCountry);
            return Json(countryRepository.GetCountries());
        }
        catch (Exception ex)
        {
            // Логирование ошибки
            Console.WriteLine("Error updating Country: " + ex.Message);
            return Json(new { error = ex.Message });
        }
    }
}
