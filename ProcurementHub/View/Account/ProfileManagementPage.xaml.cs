using ProcurementHub.ViewModel.AccountViewModels;

namespace ProcurementHub.View.Account;

public partial class ProfileManagementPage : ContentPage
{
	public ProfileManagementPage(ProfileManagementViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}