using ProcurementHub.ViewModel.TeamsViewModels.TeamRestaurantsViewModels;

namespace ProcurementHub.View.Teams.TeamRestaurants;

public partial class TeamRestaurantsPage : ContentPage
{
	public TeamRestaurantsPage(TeamRestaurantsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}