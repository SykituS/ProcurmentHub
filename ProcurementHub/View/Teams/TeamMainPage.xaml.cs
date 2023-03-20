using ProcurementHub.ViewModel.TeamsViewModels;

namespace ProcurementHub.View.Teams;

public partial class TeamMainPage : ContentPage
{
	public TeamMainPage(TeamMainViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}