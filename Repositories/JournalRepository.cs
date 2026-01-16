using DailyInk.Data;
using DailyInk.Models;

namespace DailyInk.Repositories;

public class JournalRepository
{
    private readonly AppDatabase _db;

    public JournalRepository(AppDatabase db)
    {
        _db = db;
    }

    public JournalEntry? GetTodayEntry()
        => _db.GetTodayEntry();

    public List<JournalEntry> GetAll()
        => _db.GetAllEntries();

    public JournalEntry? GetByDate(DateTime date)
    {
        return _db.GetAllEntries()
                  .FirstOrDefault(e => e.EntryDate == date.Date);
    }

    // SEARCH
    public List<JournalEntry> Search(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return GetAll();

        keyword = keyword.ToLower();

        return _db.GetAllEntries()
            .Where(e =>
                (!string.IsNullOrEmpty(e.Title) &&
                 e.Title.ToLower().Contains(keyword)) ||
                e.Content.ToLower().Contains(keyword))
            .ToList();
    }

    public List<JournalEntry> GetPaged(int page, int pageSize)
    {
        return _db.GetAllEntries()
            .OrderByDescending(e => e.EntryDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }

    public int GetCount()
    {
        return _db.GetAllEntries().Count;
    }


    public List<JournalEntry> SearchAndFilter(
    string keyword,
    DateTime? fromDate,
    DateTime? toDate,
    string? mood,
    string? tag)
    {
        var query = _db.GetAllEntries().AsQueryable();

        // Title / Content search
        if (!string.IsNullOrWhiteSpace(keyword))
        {
            keyword = keyword.ToLower();
            query = query.Where(e =>
                (!string.IsNullOrEmpty(e.Title) &&
                 e.Title.ToLower().Contains(keyword)) ||
                e.Content.ToLower().Contains(keyword));
        }

        // Date range filter
        if (fromDate.HasValue)
            query = query.Where(e => e.EntryDate >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(e => e.EntryDate <= toDate.Value);

        // Mood filter (primary)
        if (!string.IsNullOrWhiteSpace(mood))
            query = query.Where(e => e.PrimaryMood == mood);

        // Tag filter (comma-separated)
        if (!string.IsNullOrWhiteSpace(tag))
            query = query.Where(e =>
                !string.IsNullOrEmpty(e.Tags) &&
                e.Tags.ToLower().Contains(tag.ToLower()));

        return query
            .OrderByDescending(e => e.EntryDate)
            .ToList();
    }

    public List<JournalEntry> GetByDateRange(DateTime from, DateTime to)
    {
        return _db.GetAllEntries()
            .Where(e => e.EntryDate >= from.Date && e.EntryDate <= to.Date)
            .OrderBy(e => e.EntryDate)
            .ToList();
    }


    public void SaveToday(JournalEntry entry)
        => _db.SaveToday(entry);

    public void DeleteToday()
        => _db.DeleteToday();
}
