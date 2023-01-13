namespace ProcurementHub.View.Account;

public partial class LoginPage : ContentPage
{
    public LoginPage(UsersViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}