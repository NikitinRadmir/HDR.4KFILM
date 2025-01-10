namespace MyHttpServer;

/// <summary>
/// Представляет статический класс для управления сессиями пользователей.
/// </summary>
public static class SessionStorage
{
    private static readonly Dictionary<string, string> _sessions = new Dictionary<string, string>();
    private static readonly Dictionary<string, string> ReturnUrls = new Dictionary<string, string>();

    /// <summary>
    /// Сохраняет токен и его соответствующий идентификатор пользователя.
    /// </summary>
    /// <param name="token">Токен сессии.</param>
    /// <param name="userId">Идентификатор пользователя.</param>
    public static void SaveSession(string token, string userId)
    {
        _sessions[token] = userId;
    }

    /// <summary>
    /// Сохраняет URL возврата для токена.
    /// </summary>
    /// <param name="token">Токен сессии.</param>
    /// <param name="returnUrl">URL возврата.</param>
    public static void SaveReturnUrl(string token, string returnUrl)
    {
        ReturnUrls[token] = returnUrl;
    }

    /// <summary>
    /// Получает URL возврата для токена.
    /// </summary>
    /// <param name="token">Токен сессии.</param>
    /// <returns>URL возврата или null, если URL не найден.</returns>
    public static string GetReturnUrl(string token)
    {
        if (ReturnUrls.TryGetValue(token, out var returnUrl))
        {
            ReturnUrls.Remove(token); // Удаляем URL после использования
            return returnUrl;
        }
        return null;
    }

    /// <summary>
    /// Проверяет валидность токена.
    /// </summary>
    /// <param name="token">Токен сессии.</param>
    /// <returns>True, если токен валиден, иначе false.</returns>
    public static bool ValidateToken(string token)
    {
        return _sessions.ContainsKey(token);
    }

    /// <summary>
    /// Получает идентификатор пользователя по токену.
    /// </summary>
    /// <param name="token">Токен сессии.</param>
    /// <returns>Идентификатор пользователя или null, если токен не найден.</returns>
    public static string GetUserId(string token)
    {
        return _sessions.TryGetValue(token, out var userId) ? userId : null;
    }
}
