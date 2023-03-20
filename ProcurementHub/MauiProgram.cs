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
using ProcurementHub.View.Teams;
using ProcurementHub.ViewModel.TeamsViewModels;

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

        builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
        builder.Services.AddSingleton<IGeolocation>(Geolocation.Default);
        builder.Services.AddSingleton<IMap>(Map.Default);

        #region User

        builder.Services.AddTransient<UsersService>();
        builder.Services.AddTransient<UsersViewModel>();

        #endregion

        #region Person

        builder.Services.AddTransient<PersonsViewModel>();
        builder.Services.AddTransient<PersonsService>();

        #endregion

        #region Login

        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<LoginService>();
        builder.Services.AddTransient<LoginPage>();

        #endregion

        #region Register

        builder.Services.AddTransient<RegisterViewModel>();
        builder.Services.AddTransient<RegisterServices>();
        builder.Services.AddTransient<RegisterPage>();

        #endregion

        #region ForgotPassword

        builder.Services.AddTransient<ForgotPasswordPage>();
        builder.Services.AddTransient<ForgotPasswordViewModel>();

        #endregion

        #region Main

        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<MainPageViewModel>();

        #endregion

        #region Teams

        builder.Services.AddTransient<TeamsService>();
        builder.Services.AddTransient<TeamMainPage>();

        builder.Services.AddTransient<CreateNewTeamPage>();
        builder.Services.AddTransient<JoinTeamPage>();

        builder.Services.AddTransient<TeamMainViewModel>();
        builder.Services.AddTransient<JoinTeamViewModel>();
        builder.Services.AddTransient<CreateNewTeamViewModel>();

        #endregion

        #region Loading

        builder.Services.AddTransient<LoadingPage>();
        builder.Services.AddTransient<LoadingPageViewModel>();

        #endregion

        builder.Services.AddSingleton<AppShellViewModel>();


        return builder.Build();
	}
}
