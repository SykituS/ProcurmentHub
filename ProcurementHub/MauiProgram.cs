using AutoMapper;
using Grpc.Net.Client;
using GrpcShared;
using GrpcShared.Models;
using Microsoft.Extensions.Logging;
using ProcurementHub.Infrastructure;
using ProcurementHub.Services;
using ProcurementHub.View;
using ProcurementHub.View.Account;
using ProcurementHub.View.Main;
namespace ProcurementHub;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        const string baseAddress = "https://localhost:7170";

        var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
        builder.Services.AddScoped(services =>
        {
            var baseUri = new Uri(baseAddress);
            var channel = GrpcChannel.ForAddress(baseUri);
            return new Procurement.ProcurementClient(channel);
        });


        #region User

        builder.Services.AddSingleton<UsersService>();
        builder.Services.AddSingleton<UsersViewModel>();

        #endregion

        #region Person

        builder.Services.AddSingleton<PersonsViewModel>();
        builder.Services.AddSingleton<PersonsService>();

        #endregion

        #region Login

        builder.Services.AddSingleton<LoginViewModel>();
        builder.Services.AddSingleton<LoginService>();
        builder.Services.AddSingleton<LoginPage>();

        #endregion

        #region Register

        builder.Services.AddSingleton<RegisterViewModel>();
        builder.Services.AddSingleton<RegisterServices>();
        builder.Services.AddSingleton<RegisterPage>();

        #endregion

        #region ForgotPassword

        builder.Services.AddSingleton<ForgotPasswordPage>();
        builder.Services.AddSingleton<ForgotPasswordViewModel>();

        #endregion

        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<AppShellViewModel>();
        builder.Services.AddSingleton<LoadingPage>();
        builder.Services.AddSingleton<LoadingPageViewModel>();


        return builder.Build();
	}
}
