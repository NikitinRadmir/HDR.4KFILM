using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHttpServer.Models;

/// <summary>
/// Представляет модель жанра.
/// </summary>
public class Genre
{
    /// <summary>
    /// Получает или задает идентификатор жанра.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Получает или задает название жанра.
    /// </summary>
    public string GenreName { get; set; }
}
