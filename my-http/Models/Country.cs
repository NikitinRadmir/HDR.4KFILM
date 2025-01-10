using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHttpServer.Models;

/// <summary>
/// Представляет модель страны.
/// </summary>
public class Country
{
    /// <summary>
    /// Получает или задает идентификатор страны.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Получает или задает название страны.
    /// </summary>
    public string CountryName { get; set; }
}
