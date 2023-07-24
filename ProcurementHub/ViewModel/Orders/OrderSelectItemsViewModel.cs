using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrpcShared;
using ProcurementHub.ViewModel.BaseViewModels;

namespace ProcurementHub.ViewModel.Orders
{
	public partial class OrderSelectItemsViewModel : BaseViewModel
	{
		public OrderSelectItemsViewModel(Procurement.ProcurementClient procurementClient) : base(procurementClient)
		{
		}

        //TODO: Showing items for selected restaurant
		//TODO: Adding items to cart
		//TODO: Move user to sum up page
		//TODO: Add possibility to remove items from cart

    }
}
