using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcurementHub.ViewModel.Orders;

namespace ProcurementHub.View.Orders;

public partial class OrderDetailsPage : ContentPage
{
	public OrderDetailsPage(OrderDetailsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}