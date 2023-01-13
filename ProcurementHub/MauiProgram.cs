using Microsoft.Extensions.Logging;
using ProcurementHub.Services;
using ProcurementHub.View;
using ProcurementHub.View.Account;
using ProcurementHub.View.Main;

namespace ProcurementHub;

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
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

        builder.Services.AddSingleton<UsersService>();
        builder.Services.AddSingleton<UsersViewModel>();
        builder.Services.AddSingleton<LoginPage>();
        builder.Services.AddSingleton<MainPage>();

        return builder.Build();
	}
}
