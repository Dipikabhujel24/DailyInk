using DailyInk.Models;

namespace DailyInk.Repositories;

public class JournalRepository
{
    private readonly List<JournalEntry> _entries = new();

    public JournalEntry? GetTodayEntry()
        => _entries.FirstOrDefault(e => e.EntryDate == DateTime.Today);

    public void SaveToday(string content)
    {
        var entry = GetTodayEntry();

        if (entry == null)
        {
            _entries.Add(new JournalEntry
            {
                EntryId = _entries.Count + 1,
                EntryDate = DateTime.Today,
                Content = content,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            });
        }
        else
        {
            entry.Content = content;
            entry.UpdatedAt = DateTime.Now;
        }
    }

    public void DeleteToday()
    {
        var entry = GetTodayEntry();
        if (entry != null)
            _entries.Remove(entry);
    }
}
