using ProcurementHub.ViewModel.Orders;

namespace ProcurementHub.View.Orders;

public partial class OrderStartPage : ContentPage
{
	public OrderStartPage(OrderStartViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}