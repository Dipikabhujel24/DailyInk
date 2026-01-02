using SQLite;

namespace DailyInk.Models;

public class JournalEntry
{
    [PrimaryKey, AutoIncrement]
    public int EntryId { get; set; }

    public DateTime EntryDate { get; set; }

    public string Title { get; set; } = "";

    public string? PrimaryMood { get; set; }

    // Stored in SQLite
    public string SecondaryMoodsRaw { get; set; } = "";

    public string Content { get; set; } = "";

    public string Tags { get; set; } = "";

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    private List<string>? _secondaryMoods;

    [Ignore]
    public List<string> SecondaryMoods
    {
        get
        {
            if (_secondaryMoods == null)
            {
                _secondaryMoods = string.IsNullOrWhiteSpace(SecondaryMoodsRaw)
                    ? new List<string>()
                    : SecondaryMoodsRaw.Split(',').ToList();
            }
            return _secondaryMoods;
        }
        set
        {
            _secondaryMoods = value;
            SecondaryMoodsRaw = string.Join(",", value);
        }
    }
}
