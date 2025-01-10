namespace MyHttpServer.Models;

/// <summary>
/// Представляет модель данных фильма.
/// </summary>
public class MovieData
{
    /// <summary>
    /// Получает или задает уникальный идентификатор фильма.
    /// </summary>
    public int MovieId { get; set; }

    /// <summary>
    /// Получает или задает название фильма.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Получает или задает ссылку на обложку фильма.
    /// </summary>
    public string CoverImageUrl { get; set; }

    /// <summary>
    /// Получает или задает описание фильма.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Получает или задает оригинальное название фильма.
    /// </summary>
    public string OriginalTitle { get; set; }

    /// <summary>
    /// Получает или задает год выпуска фильма.
    /// </summary>
    public int Year { get; set; }

    /// <summary>
    /// Получает или задает страну производства фильма.
    /// </summary>
    public string Country { get; set; }

    /// <summary>
    /// Получает или задает жанр фильма.
    /// </summary>
    public string Genre { get; set; }

    /// <summary>
    /// Получает или задает качество фильма.
    /// </summary>
    public string Quality { get; set; }

    /// <summary>
    /// Получает или задает озвучку фильма.
    /// </summary>
    public string Sound { get; set; }

    /// <summary>
    /// Получает или задает режиссера фильма.
    /// </summary>
    public string Director { get; set; }

    /// <summary>
    /// Получает или задает актеров, снимавшихся в фильме.
    /// </summary>
    public string Cast { get; set; }

    /// <summary>
    /// Получает или задает плеер-токен фильма.
    /// </summary>
    public string MoviePlayer { get; set; }
}
