using Microsoft.Extensions.Logging;
using DailyInk.Repositories;
using DailyInk.Services;

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

            // Register Journal Services
            builder.Services.AddSingleton<JournalRepository>();
            builder.Services.AddScoped<JournalService>();
            // Theme Services
            builder.Services.AddSingleton<ThemeService>();



#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
