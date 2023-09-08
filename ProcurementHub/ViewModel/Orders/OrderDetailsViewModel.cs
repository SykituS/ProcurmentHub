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
    [QueryProperty(nameof(OrderModel), "OrderModel")]
    public partial class OrderDetailsViewModel : BaseViewModel
    {
        private OrderServices _orderServices;
        public ObservableCollection<OrderItemsModel> OrderItems { get; set; } = new();
        
		[ObservableProperty]
        private OrderModel _orderModel;

		[ObservableProperty]
		private TeamMainModel _teamModel;

		public OrderDetailsViewModel(Procurement.ProcurementClient procurementClient, OrderServices orderServices) : base(procurementClient)
        {
            _orderServices = orderServices;
        }

        [ObservableProperty]
        bool _isRefreshing;


        [RelayCommand]
        async Task GetOrderInformation()
        {
            _isRefreshing = true;

            try
            {
                var response = await _orderServices.GetFullOrderDetails(_orderModel.ID);

                if (!response.Item1.Successful)
                {
                    return;
                }

                if (OrderItems.Any())
                    OrderItems.Clear();

                _orderModel = response.Item2;

                foreach (var item in response.Item3)
                    OrderItems.Add(item);
            }
            finally
            {
                _isRefreshing = false;
            }
        }

        [RelayCommand]
        async Task GoBack()
        {

        }

        async partial void OnOrderModelChanged(OrderModel value)
        {
            await GetOrderInformation();
        }
    }
}
