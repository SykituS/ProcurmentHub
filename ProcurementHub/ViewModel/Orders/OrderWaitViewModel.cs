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
	public partial class OrderWaitViewModel : BaseViewModel
	{
		private TeamRestaurantsService _teamRestaurantsService;
		private OrderServices _orderServices;

		public ObservableCollection<OrderItemsModel> OrderItems { get; set; }

		[ObservableProperty]
		private TeamMainModel _teamModel;

		[ObservableProperty]
		private OrderModel _orderModel;

		public OrderWaitViewModel(Procurement.ProcurementClient procurementClient, TeamRestaurantsService teamRestaurantsService, OrderServices orderServices) : base(procurementClient)
		{
			_teamRestaurantsService = teamRestaurantsService;
			_orderServices = orderServices;
		}

		[ObservableProperty]
		bool _isRefreshing;

		[ObservableProperty]
		bool _isPayingPerson;

		[RelayCommand]
		async Task GetOrderInformation()
		{
			if (App.LoggedUserInApplication.PersonID != null)
				_isPayingPerson = _orderModel.OrderPayedByID == App.LoggedUserInApplication.PersonID.Value;

            var response = await _orderServices.GetFullOrderDetails(_orderModel.ID);
            
        }

        [RelayCommand]
        async Task CheckOrderForUpdate()
        {
            //send request to get new data
        }

        [RelayCommand]
		async Task ConfirmOrderDelivery()
		{

		}

		async partial void OnOrderModelChanged(OrderModel value)
		{
			await GetOrderInformation();
		}
	}
}
