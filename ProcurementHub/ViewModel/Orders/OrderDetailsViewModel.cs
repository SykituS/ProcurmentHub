using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrpcShared;
using ProcurementHub.Model.CustomModels;
using ProcurementHub.Services;
using ProcurementHub.ViewModel.BaseViewModels;

namespace ProcurementHub.ViewModel.Orders
{
    [QueryProperty(nameof(TeamModel), "TeamMainModel")]
    [QueryProperty(nameof(OrderId), "OrderId")]
    public partial class OrderDetailsViewModel : BaseViewModel
    {
        public OrderServices OrderServices;
        public ObservableCollection<OrderItemsModel> Orders { get; set; } = new();

		[ObservableProperty]
        private string _orderId;

		[ObservableProperty]
        private OrderModel _orderModel;

		[ObservableProperty]
		private TeamMainModel _teamModel;

		public OrderDetailsViewModel(Procurement.ProcurementClient procurementClient) : base(procurementClient)
		{
		}
	}
}
