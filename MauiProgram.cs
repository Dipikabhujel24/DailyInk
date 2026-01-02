using Microsoft.Extensions.Logging;
using DailyInk.Repositories;
using DailyInk.Services;
using DailyInk.Data;

namespace DailyInk
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            // Blazor WebView
            builder.Services.AddMauiBlazorWebView();

            var dbPath = Path.Combine(
                FileSystem.AppDataDirectory,
                "dailyink.db");

            builder.Services.AddSingleton<AppDatabase>(
                _ => new AppDatabase(dbPath));

            // =========================
            // REPOSITORIES & SERVICES
            // =========================
            builder.Services.AddSingleton<JournalRepository>();
            builder.Services.AddScoped<JournalService>();
            builder.Services.AddSingleton<MarkdownService>();

            // Theme Service
            builder.Services.AddSingleton<ThemeService>();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
