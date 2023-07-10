using ProcurementHub.ViewModel.TeamsViewModels.TeamRestaurantsViewModels;

namespace ProcurementHub.View.Teams.TeamRestaurants;

public partial class TeamRestaurantItemsPage : ContentPage
{
	public TeamRestaurantItemsPage(TeamRestaurantItemsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}