namespace TemlateEngine;

/// <summary>
/// ��������� ��� ������ �������� HTML.
/// </summary>
public interface IHtmlTemplateEngine
{
    /// <summary>
    /// �������� ������ � �������������� ��������� ������.
    /// </summary>
    /// <param name="template">������ HTML.</param>
    /// <param name="data">��������� ������ ��� ����������.</param>
    /// <returns>������������� HTML.</returns>
    string Render(string template, string data);

    /// <summary>
    /// �������� ������ � �������������� ������� ������.
    /// </summary>
    /// <param name="template">������ HTML.</param>
    /// <param name="obj">������ ������ ��� ����������.</param>
    /// <returns>������������� HTML.</returns>
    string Render(string template, object obj);
}
