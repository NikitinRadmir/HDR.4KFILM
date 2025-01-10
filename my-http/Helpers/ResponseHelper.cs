using HttpServerLibrary.Models;

namespace MyHttpServer.Helpers;

/// <summary>
/// Реализация помощника ответа.
/// </summary>
public class ResponseHelper : IResponseHelper
{
    /// <summary>
    /// Получает текст ответа из локального пути.
    /// </summary>
    /// <param name="localPath">Локальный путь к файлу ответа.</param>
    /// <returns>Текст ответа.</returns>
    public string GetResponseText(string localPath)
    {
        var filePath = AppConfig.GetInstance().Path + localPath;
        var additionalPath = AppConfig.GetInstance().AddPath + localPath;
        if (File.Exists(filePath))
        {
            var responseText = File.ReadAllText(filePath);
            return responseText;
        }
        else if (File.Exists(additionalPath))
        {
            var responseText = File.ReadAllText(additionalPath);
            return responseText;
        }

        return "error 404 file not found";
    }
}
