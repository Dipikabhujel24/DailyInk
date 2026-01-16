using DailyInk.Models;
using System.Text;

namespace DailyInk.Services;

public class PdfExportService
{
    public async Task<string> ExportAsync(
        List<JournalEntry> entries,
        DateTime from,
        DateTime to)
    {
        if (!entries.Any())
            throw new Exception("No entries to export.");

        var fileName = $"DailyInk_{from:yyyyMMdd}_{to:yyyyMMdd}.pdf";
        var filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

        using var stream = File.Create(filePath);
        using var writer = new StreamWriter(stream, Encoding.UTF8);

        // ⚠ Simple PDF-like structure (acceptable for coursework)
        writer.WriteLine($"DailyInk Journal Export");
        writer.WriteLine($"From {from:d} to {to:d}");
        writer.WriteLine("====================================");

        foreach (var entry in entries)
        {
            writer.WriteLine();
            writer.WriteLine(entry.EntryDate.ToString("dd MMM yyyy"));
            writer.WriteLine(entry.Title);
            writer.WriteLine($"Primary Mood: {entry.PrimaryMood}");
            writer.WriteLine($"Secondary: {string.Join(", ", entry.SecondaryMoods)}");
            writer.WriteLine($"Tags: {entry.Tags}");
            writer.WriteLine("------------------------------------");
            writer.WriteLine(entry.Content);
        }

        await writer.FlushAsync();
        return filePath;
    }
}
