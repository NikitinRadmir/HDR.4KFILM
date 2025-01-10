namespace MyHttpServer.Models;

/// <summary>
/// ������������ ������ ������ ������.
/// </summary>
public class MovieData
{
    /// <summary>
    /// �������� ��� ������ ���������� ������������� ������.
    /// </summary>
    public int MovieId { get; set; }

    /// <summary>
    /// �������� ��� ������ �������� ������.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// �������� ��� ������ ������ �� ������� ������.
    /// </summary>
    public string CoverImageUrl { get; set; }

    /// <summary>
    /// �������� ��� ������ �������� ������.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// �������� ��� ������ ������������ �������� ������.
    /// </summary>
    public string OriginalTitle { get; set; }

    /// <summary>
    /// �������� ��� ������ ��� ������� ������.
    /// </summary>
    public int Year { get; set; }

    /// <summary>
    /// �������� ��� ������ ������ ������������ ������.
    /// </summary>
    public string Country { get; set; }

    /// <summary>
    /// �������� ��� ������ ���� ������.
    /// </summary>
    public string Genre { get; set; }

    /// <summary>
    /// �������� ��� ������ �������� ������.
    /// </summary>
    public string Quality { get; set; }

    /// <summary>
    /// �������� ��� ������ ������� ������.
    /// </summary>
    public string Sound { get; set; }

    /// <summary>
    /// �������� ��� ������ ��������� ������.
    /// </summary>
    public string Director { get; set; }

    /// <summary>
    /// �������� ��� ������ �������, ����������� � ������.
    /// </summary>
    public string Cast { get; set; }

    /// <summary>
    /// �������� ��� ������ �����-����� ������.
    /// </summary>
    public string MoviePlayer { get; set; }
}
