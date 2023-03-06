using GrpcShared.Models;

namespace ProcurementHub.View.Main;

public partial class MainPage : ContentPage
{
	public MainPage(UsersViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }


}

