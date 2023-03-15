using GrpcShared.Models;

namespace ProcurementHub.View.Main;

public partial class MainPage : ContentPage
{
	public MainPage(MainPageViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }


}

