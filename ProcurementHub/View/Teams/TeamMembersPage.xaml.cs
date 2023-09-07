using ProcurementHub.ViewModel.TeamsViewModels;

namespace ProcurementHub.View.Teams;

public partial class TeamMembersPage : ContentPage
{
	public TeamMembersPage(TeamMembersViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}