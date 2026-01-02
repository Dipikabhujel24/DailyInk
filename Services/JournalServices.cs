using DailyInk.Models;
using DailyInk.Repositories;

namespace DailyInk.Services;

public class JournalService
{
    private readonly JournalRepository _repo;

    public JournalService(JournalRepository repo)
    {
        _repo = repo;
    }

    public JournalEntry? GetTodayEntry()
        => _repo.GetTodayEntry();

    public List<JournalEntry> GetAllEntries()
        => _repo.GetAll();

    public JournalEntry? GetEntryByDate(DateTime date)
    {
        return _repo.GetByDate(date);
    }


    // SEARCH
    public List<JournalEntry> Search(string keyword)
        => _repo.Search(keyword);

    public void SaveTodayEntry(JournalEntry entry)
        => _repo.SaveToday(entry);

    public void DeleteTodayEntry()
        => _repo.DeleteToday();

    public List<JournalEntry> SearchAndFilter(
        string keyword,
        DateTime? fromDate,
        DateTime? toDate,
        string? mood,
        string? tag)
    {
        return _repo.SearchAndFilter(keyword, fromDate, toDate, mood, tag);
    }
    public int GetCurrentStreak()
    {
        var entries = GetAllEntries();
        int streak = 0;
        var date = DateTime.Today;

        while (entries.Any(e => e.EntryDate == date))
        {
            streak++;
            date = date.AddDays(-1);
        }

        return streak;
    }

    public int GetLongestStreak()
    {
        var entries = GetAllEntries()
            .OrderBy(e => e.EntryDate)
            .ToList();

        int longest = 0;
        int current = 0;

        for (int i = 0; i < entries.Count; i++)
        {
            if (i == 0 || entries[i].EntryDate == entries[i - 1].EntryDate.AddDays(1))
            {
                current++;
            }
            else
            {
                current = 1;
            }

            longest = Math.Max(longest, current);
        }

        return longest;
    }

    public List<DateTime> GetMissedDays()
    {
        var entries = GetAllEntries();
        if (!entries.Any())
            return new List<DateTime>();

        var datesWithEntries = entries
            .Select(e => e.EntryDate)
            .Distinct()
            .ToHashSet();

        var firstDate = entries.Min(e => e.EntryDate);
        var today = DateTime.Today;

        var missedDays = new List<DateTime>();

        for (var date = firstDate; date <= today; date = date.AddDays(1))
        {
            if (!datesWithEntries.Contains(date))
                missedDays.Add(date);
        }

        return missedDays;
    }

    public Dictionary<string, int> GetPrimaryMoodDistribution()
    {
        return GetAllEntries()
            .Where(e => !string.IsNullOrWhiteSpace(e.PrimaryMood))
            .GroupBy(e => e.PrimaryMood!)
            .ToDictionary(g => g.Key, g => g.Count());
    }

    public Dictionary<string, int> GetSecondaryMoodDistribution()
    {
        return GetAllEntries()
            .SelectMany(e => e.SecondaryMoods)
            .GroupBy(m => m)
            .ToDictionary(g => g.Key, g => g.Count());
    }

    public Dictionary<string, int> GetTagUsage()
    {
        return GetAllEntries()
            .Where(e => !string.IsNullOrWhiteSpace(e.Tags))
            .SelectMany(e => e.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries))
            .Select(t => t.Trim())
            .GroupBy(t => t)
            .ToDictionary(g => g.Key, g => g.Count());
    }

    public Dictionary<DateTime, int> GetWordCountByDate()
    {
        return GetAllEntries()
            .Where(e => !string.IsNullOrWhiteSpace(e.Content))
            .GroupBy(e => e.EntryDate)
            .ToDictionary(
                g => g.Key,
                g => g.Sum(e => e.Content.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length)
            );
    }

}