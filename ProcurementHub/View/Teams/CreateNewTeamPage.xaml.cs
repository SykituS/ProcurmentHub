using ProcurementHub.ViewModel.TeamsViewModels;

namespace ProcurementHub.View.Teams;

public partial class CreateNewTeamPage : ContentPage
{
	public CreateNewTeamPage(CreateNewTeamViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}