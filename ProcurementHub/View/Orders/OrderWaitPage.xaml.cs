using ProcurementHub.ViewModel.Orders;

namespace ProcurementHub.View.Orders;

public partial class OrderWaitPage : ContentPage
{
	public OrderWaitPage(OrderWaitViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}