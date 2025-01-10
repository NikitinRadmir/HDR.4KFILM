namespace MyHttpServer.Models;

/// <summary>
/// Представляет модель пользователя.
/// </summary>
public class User
{
    /// <summary>
    /// Получает или задает идентификатор пользователя.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Получает или задает логин пользователя.
    /// </summary>
    public string Login { get; set; }

    /// <summary>
    /// Получает или задает пароль пользователя.
    /// </summary>
    public string Password { get; set; }
}
