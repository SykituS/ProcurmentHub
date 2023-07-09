using ForgotPasswordViewModel = ProcurementHub.ViewModel.AccountViewModels.ForgotPasswordViewModel;

namespace ProcurementHub.View.Account;

public partial class ForgotPasswordPage : ContentPage
{
	public ForgotPasswordPage(ForgotPasswordViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}