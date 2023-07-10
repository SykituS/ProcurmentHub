using GrpcShared;
using ProcurementHub.View.Account;
using ProcurementHub.View.Main;
using ProcurementHub.View.Teams;
using ProcurementHub.View.Teams.TeamRestaurants;
using TeamRestaurants = ProcurementHub.Model.Models.TeamRestaurants;

namespace ProcurementHub;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
		Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
		Routing.RegisterRoute(nameof(ForgotPasswordPage), typeof(ForgotPasswordPage));

		Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
		Routing.RegisterRoute(nameof(TeamMainPage), typeof(TeamMainPage));
		Routing.RegisterRoute(nameof(JoinTeamPage), typeof(JoinTeamPage));
		Routing.RegisterRoute(nameof(CreateNewTeamPage), typeof(CreateNewTeamPage));
		Routing.RegisterRoute(nameof(ProfileManagementPage), typeof(ProfileManagementPage));

		Routing.RegisterRoute(nameof(TeamSettingsPage), typeof(TeamSettingsPage));
		Routing.RegisterRoute(nameof(TeamRestaurantsPage), typeof(TeamRestaurantsPage));
		Routing.RegisterRoute(nameof(TeamRestaurantsAddEditPage), typeof(TeamRestaurantsAddEditPage));
		Routing.RegisterRoute(nameof(TeamRestaurantItemsPage), typeof(TeamRestaurantItemsPage));
		Routing.RegisterRoute(nameof(TeamRestaurantItemAddEditPage), typeof(TeamRestaurantItemAddEditPage));



	}
}
