namespace TemlateEngine;

/// <summary>
/// Интерфейс для движка шаблонов HTML.
/// </summary>
public interface IHtmlTemplateEngine
{
    /// <summary>
    /// Рендерит шаблон с использованием строковых данных.
    /// </summary>
    /// <param name="template">Шаблон HTML.</param>
    /// <param name="data">Строковые данные для рендеринга.</param>
    /// <returns>Отрендеренный HTML.</returns>
    string Render(string template, string data);

    /// <summary>
    /// Рендерит шаблон с использованием объекта данных.
    /// </summary>
    /// <param name="template">Шаблон HTML.</param>
    /// <param name="obj">Объект данных для рендеринга.</param>
    /// <returns>Отрендеренный HTML.</returns>
    string Render(string template, object obj);
}
