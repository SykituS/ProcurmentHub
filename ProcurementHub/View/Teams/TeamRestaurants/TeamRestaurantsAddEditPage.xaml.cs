using ProcurementHub.ViewModel.TeamsViewModels.TeamRestaurantsViewModels;

namespace ProcurementHub.View.Teams.TeamRestaurants;

public partial class TeamRestaurantsAddEditPage : ContentPage
{
	public TeamRestaurantsAddEditPage(TeamRestaurantsAddEditViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}