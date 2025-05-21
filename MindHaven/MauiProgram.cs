using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Syncfusion.Maui.Core.Hosting; // Syncfusion Registration
using CommunityToolkit.Maui; // CommunityToolkit Registration

namespace MindHaven
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureSyncfusionCore() // Register Syncfusion handlers
                .UseMauiCommunityToolkit() // Register CommunityToolkit
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("InknutAntiqua-Regular.ttf", "InknutAntiqua");
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            return builder.Build();
        }
    }
}
