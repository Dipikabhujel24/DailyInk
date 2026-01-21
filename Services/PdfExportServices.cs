using DailyInk.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace DailyInk.Services;

public class PdfExportService
{
    public async Task<string> ExportAsync(
        List<JournalEntry> entries,
        DateTime from,
        DateTime to)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        var fileName = $"DailyInk_{from:yyyyMMdd}_{to:yyyyMMdd}.pdf";
        var filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(30);
                page.DefaultTextStyle(x => x.FontSize(11));

                page.Content().Column(col =>
                {
                    col.Item().Text("DailyInk Journal Export")
                        .FontSize(18)
                        .Bold();

                    col.Item()
                        .PaddingBottom(10)
                        .Text($"From {from:d} to {to:d}")
                        .Italic();

                    foreach (var entry in entries)
                    {
                        col.Item()
                            .PaddingVertical(8)
                            .BorderBottom(1)
                            .Column(e =>
                            {
                                e.Item().Text(entry.EntryDate.ToString("dd MMM yyyy"))
                                    .Bold();

                                if (!string.IsNullOrWhiteSpace(entry.Title))
                                    e.Item().Text(entry.Title).Bold();

                                e.Item().Text($"Primary Mood: {entry.PrimaryMood}");

                                if (entry.SecondaryMoods.Any())
                                    e.Item().Text($"Secondary Moods: {string.Join(", ", entry.SecondaryMoods)}");

                                if (!string.IsNullOrWhiteSpace(entry.Tags))
                                    e.Item().Text($"Tags: {entry.Tags}");

                                if (!string.IsNullOrWhiteSpace(entry.Content))
                                {
                                    e.Item()
                                        .PaddingTop(5)
                                        .Text(entry.Content);
                                }
                            });
                    }
                });
            });
        })
        .GeneratePdf(filePath);

        await Task.CompletedTask;
        return filePath;
    }
}
