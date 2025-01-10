namespace MyHttpServer.Helpers;

/// <summary>
/// ��������� ��� ��������� ������.
/// </summary>
public interface IResponseHelper
{
    /// <summary>
    /// �������� ����� ������ �� ���������� ����.
    /// </summary>
    /// <param name="localPath">��������� ���� � ����� ������.</param>
    /// <returns>����� ������.</returns>
    string GetResponseText(string localPath);
}
