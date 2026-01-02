using SQLite;
using DailyInk.Models;

namespace DailyInk.Data;

public class AppDatabase
{
    private readonly SQLiteConnection _db;

    public AppDatabase(string dbPath)
    {
        _db = new SQLiteConnection(dbPath);
        _db.CreateTable<JournalEntry>();
    }

    public JournalEntry? GetTodayEntry()
        => _db.Table<JournalEntry>()
              .FirstOrDefault(e => e.EntryDate == DateTime.Today);

    public List<JournalEntry> GetAllEntries()
        => _db.Table<JournalEntry>()
              .OrderByDescending(e => e.EntryDate)
              .ToList();

    public void SaveToday(JournalEntry entry)
    {
        var existing = GetTodayEntry();

        if (existing == null)
        {
            entry.EntryDate = DateTime.Today;
            entry.CreatedAt = DateTime.Now;
            entry.UpdatedAt = DateTime.Now;

            _db.Insert(entry);
        }
        else
        {
            existing.Title = entry.Title;
            existing.Content = entry.Content;
            existing.PrimaryMood = entry.PrimaryMood;
            existing.SecondaryMoodsRaw = entry.SecondaryMoodsRaw;
            existing.UpdatedAt = DateTime.Now;

            _db.Update(existing);
        }
    }

    public void DeleteToday()
    {
        var entry = GetTodayEntry();
        if (entry != null)
            _db.Delete(entry);
    }
}
