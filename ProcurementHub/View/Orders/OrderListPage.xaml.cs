using ProcurementHub.ViewModel.Orders;

namespace ProcurementHub.View.Orders;

public partial class OrderListPage : ContentPage
{
	public OrderListPage(OrderListViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}