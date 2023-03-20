using ProcurementHub.ViewModel.TeamsViewModels;

namespace ProcurementHub.View.Teams;

public partial class JoinTeamPage : ContentPage
{
	public JoinTeamPage(JoinTeamViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}