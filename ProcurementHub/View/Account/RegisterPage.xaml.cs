using RegisterViewModel = ProcurementHub.ViewModel.AccountViewModels.RegisterViewModel;

namespace ProcurementHub.View.Account;

public partial class RegisterPage : ContentPage
{
	public RegisterPage(RegisterViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}