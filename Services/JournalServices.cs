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

    public void SaveTodayEntry(string content)
        => _repo.SaveToday(content);

    public void DeleteTodayEntry()
        => _repo.DeleteToday();
}
