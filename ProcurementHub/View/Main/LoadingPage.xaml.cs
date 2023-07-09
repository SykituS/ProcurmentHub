using ProcurementHub.ViewModel.MainViewModels;

namespace ProcurementHub.View.Main;

public partial class LoadingPage : ContentPage
{
	public LoadingPage(LoadingPageViewModel viewModel)
	{
        InitializeComponent();
        BindingContext = viewModel;
    }
}