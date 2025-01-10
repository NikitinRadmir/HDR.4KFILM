namespace MyHttpServer.Models;

/// <summary>
/// ������������ ������ ������������.
/// </summary>
public class User
{
    /// <summary>
    /// �������� ��� ������ ������������� ������������.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// �������� ��� ������ ����� ������������.
    /// </summary>
    public string Login { get; set; }

    /// <summary>
    /// �������� ��� ������ ������ ������������.
    /// </summary>
    public string Password { get; set; }
}
