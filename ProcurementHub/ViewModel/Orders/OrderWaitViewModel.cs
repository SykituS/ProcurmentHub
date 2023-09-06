using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;
using GrpcShared;
using ProcurementHub.Controls;
using ProcurementHub.Model.CustomModels;
using ProcurementHub.Model.Enums;
using ProcurementHub.Services;
using ProcurementHub.View.Orders;
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
        async Task CheckOrderForUpdate()
        {
            //send request to get new data
        }

        [RelayCommand]
        async Task OrderDelivery()
        {
            activePopUp = OrderWaitControl.GeneratePopupForOrderDeliveryConfirmation(ConfirmOrderDeliveryCommand);
            await App.Current.MainPage.ShowPopupAsync(activePopUp);
        }

        [RelayCommand]
        async Task GoToOrderDetails()
        {
            await Shell.Current.GoToAsync(nameof(OrderDetailsPage), true, new Dictionary<string, object>
            {
                {"TeamMainModel", _teamModel },
                {"OrderId", _orderModel.ID },
            });
        }

        [RelayCommand]
        async Task ConfirmOrderDelivery(bool isFinished)
        {
            if (!isFinished)
            {
                activePopUp.Close();
                return;
            }

            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                await SnackBarControl.CreateSnackBar("Closing order");

                var result = await _orderServices.CloseOrder(_orderModel.ID);

                if (result.Successful)
                {
                    await Shell.Current.GoToAsync(nameof(OrderDetailsPage), true, new Dictionary<string, object>
                    {
                        {"TeamMainModel", _teamModel },
                        {"OrderId", _orderModel.ID },
                    });
                }
                else
                {
                    await SnackBarControl.CreateSnackBar(result.Information);
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        async partial void OnOrderModelChanged(OrderModel value)
        {
            await GetOrderInformation();
        }
    }
}
