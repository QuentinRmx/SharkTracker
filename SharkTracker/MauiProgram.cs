using Microsoft.AspNetCore.Components.WebView.Maui;
using SharkTracker.Data;
using SharkTracker.Data.Models;

namespace SharkTracker;

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

		builder.Services.AddMauiBlazorWebView();
		#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
#endif

        string dbPath = FileAccessHelper.GetLocalFilePath("sharktracker.db3");
        builder.Services.AddSingleton(s => ActivatorUtilities.CreateInstance<Repository<Deck>>(s, dbPath));
        builder.Services.AddSingleton(s => ActivatorUtilities.CreateInstance<Repository<Matchup>>(s, dbPath));

        return builder.Build();
	}
}
