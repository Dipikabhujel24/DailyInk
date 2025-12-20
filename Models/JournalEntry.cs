namespace DailyInk.Models;

public class JournalEntry
{
    public int EntryId { get; set; }
    public DateTime EntryDate { get; set; }
    public string Title { get; set; } = "";

    public string? PrimaryMood { get; set; }

    public List<string> SecondaryMoods { get; set; } = new();
    public string Content { get; set; } = "";

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
