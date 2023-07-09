using LoginViewModel = ProcurementHub.ViewModel.AccountViewModels.LoginViewModel;

namespace ProcurementHub.View.Account;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}