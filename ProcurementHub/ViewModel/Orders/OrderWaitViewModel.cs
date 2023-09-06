using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrpcShared;
using ProcurementHub.Model.CustomModels;
using ProcurementHub.Model.Enums;
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

        public ObservableCollection<OrderItemsModel> OrderItems { get; set; } = new();

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

        [ObservableProperty]
        bool _isNotPayingPerson;

        [ObservableProperty]
        bool _isOrderFinished;

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
                
			    if (App.LoggedUserInApplication.PersonID != null)
				    _isPayingPerson = _orderModel.OrderPayedByID == App.LoggedUserInApplication.PersonID.Value;

                _isNotPayingPerson = !_isPayingPerson;

                if (OrderModel.Status == TeamOrderStatusEnum.Closed)
                {
                    _isOrderFinished = true;
					_isNotPayingPerson = false;
                    _isPayingPerson = false;
                }
            }
            finally
            {
			    _isRefreshing = false;
            }
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

        [RelayCommand]
        async Task GoToOrderDetails()
        {

        }

        async partial void OnOrderModelChanged(OrderModel value)
		{
			await GetOrderInformation();
		}
	}
}
