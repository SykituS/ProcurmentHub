﻿using AutoMapper;
using Grpc.Net.Client;
using GrpcShared;
using Microsoft.Extensions.Logging;
using ProcurementHub.Infrastructure;
using ProcurementHub.Services;
using ProcurementHub.View;
using ProcurementHub.View.Account;
using ProcurementHub.View.Main;
using ProcurementHub.View.Teams;
using ProcurementHub.View.Teams.TeamRestaurants;
using ProcurementHub.ViewModel.AccountViewModels;
using ProcurementHub.ViewModel.MainViewModels;
using ProcurementHub.ViewModel.TeamsViewModels;
using ProcurementHub.ViewModel.TeamsViewModels.TeamRestaurantsViewModels;
using ForgotPasswordViewModel = ProcurementHub.ViewModel.AccountViewModels.ForgotPasswordViewModel;
using LoginViewModel = ProcurementHub.ViewModel.AccountViewModels.LoginViewModel;
using MainPageViewModel = ProcurementHub.ViewModel.MainViewModels.MainPageViewModel;
using RegisterViewModel = ProcurementHub.ViewModel.AccountViewModels.RegisterViewModel;

namespace ProcurementHub;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		string baseAddress = DeviceInfo.Platform == DevicePlatform.Android
			? "https://0.0.0.0:7170"
			: "https://localhost:7170";


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

		#region LoggedUserInApplication

		builder.Services.AddTransient<UsersService>();
		builder.Services.AddTransient<UsersViewModel>();

		#endregion

		#region Person

		builder.Services.AddTransient<PersonsViewModel>();
		builder.Services.AddTransient<PersonsService>();

		#endregion

		#region Account

		builder.Services.AddTransient<LoginViewModel>();
		builder.Services.AddTransient<LoginService>();
		builder.Services.AddTransient<LoginPage>();

		builder.Services.AddTransient<RegisterViewModel>();
		builder.Services.AddTransient<RegisterServices>();
		builder.Services.AddTransient<RegisterPage>();

		builder.Services.AddTransient<ForgotPasswordPage>();
		builder.Services.AddTransient<ForgotPasswordViewModel>();

		builder.Services.AddTransient<ProfileManagementPage>();
		builder.Services.AddTransient<ProfileManagementViewModel>();

		#endregion

		#region Main

		builder.Services.AddTransient<MainPage>();
		builder.Services.AddTransient<MainPageViewModel>();

		builder.Services.AddTransient<LoadingPage>();
		builder.Services.AddTransient<LoadingPageViewModel>();

		#endregion

		#region Teams


		builder.Services.AddTransient<TeamsService>();

		builder.Services.AddTransient<TeamMainPage>();
		builder.Services.AddTransient<TeamSettingsPage>();
		builder.Services.AddTransient<CreateNewTeamPage>();
		builder.Services.AddTransient<JoinTeamPage>();

		builder.Services.AddTransient<TeamMainViewModel>();
		builder.Services.AddTransient<JoinTeamViewModel>();
		builder.Services.AddTransient<TeamSettingsViewModel>();
		builder.Services.AddTransient<CreateNewTeamViewModel>();

		#endregion

		#region TeamRestaurantsPage

		builder.Services.AddTransient<TeamRestaurantsService>();

		builder.Services.AddTransient<TeamRestaurantsPage>();
		builder.Services.AddTransient<TeamRestaurantsAddEditPage>();
		builder.Services.AddTransient<TeamRestaurantItemsPage>();
		builder.Services.AddTransient<TeamRestaurantItemAddEditPage>();

		builder.Services.AddTransient<TeamRestaurantsViewModel>();
		builder.Services.AddTransient<TeamRestaurantsAddEditViewModel>();
		builder.Services.AddTransient<TeamRestaurantItemsViewModel>();
		builder.Services.AddTransient<TeamRestaurantItemAddEditViewModel>();

		#endregion

		//TODO: Register order pages and create models and view models for them

		return builder.Build();
	}
}
