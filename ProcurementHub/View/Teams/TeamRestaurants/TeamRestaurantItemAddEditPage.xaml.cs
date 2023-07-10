using ProcurementHub.ViewModel.TeamsViewModels.TeamRestaurantsViewModels;

namespace ProcurementHub.View.Teams.TeamRestaurants;

public partial class TeamRestaurantItemAddEditPage : ContentPage
{
	public TeamRestaurantItemAddEditPage(TeamRestaurantItemAddEditViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}