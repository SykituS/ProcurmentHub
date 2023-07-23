using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrpcShared;
using ProcurementHub.ViewModel.BaseViewModels;

namespace ProcurementHub.ViewModel.Orders
{
	public partial class OrderSelectPayingPersonViewModel : BaseViewModel
	{
		public OrderSelectPayingPersonViewModel(Procurement.ProcurementClient procurementClient) : base(procurementClient)
		{
		}
	}
}
