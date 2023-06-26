using ProcurementHub.ViewModel.TeamsViewModels;

namespace ProcurementHub.View.Teams;

public partial class TeamSettingsPage : ContentPage
{
	public TeamSettingsPage(TeamSettingsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}