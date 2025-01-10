namespace MyHttpServer.Helpers;

/// <summary>
/// Интерфейс для помощника ответа.
/// </summary>
public interface IResponseHelper
{
    /// <summary>
    /// Получает текст ответа из локального пути.
    /// </summary>
    /// <param name="localPath">Локальный путь к файлу ответа.</param>
    /// <returns>Текст ответа.</returns>
    string GetResponseText(string localPath);
}
