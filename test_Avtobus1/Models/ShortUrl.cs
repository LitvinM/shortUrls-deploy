namespace test_Avtobus1.Models;


public class ShortUrl
{
    public int Id { get; set; }
    public string OriginalUrl { get; set; } = null!;
    public string ShortCode { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public int ClickCount { get; set; }
}
