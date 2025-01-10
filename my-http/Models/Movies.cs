using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHttpServer.Models;

/// <summary>
/// Представляет модель фильма.
/// </summary>
public class Movie
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
    /// Получает или задает URL изображения фильма.
    /// </summary>
    public string ImageUrl { get; set; }

    /// <summary>
    /// Получает или задает URL HTML-страницы фильма.
    /// </summary>
    public string HtmlPageUrl { get; set; }
}
