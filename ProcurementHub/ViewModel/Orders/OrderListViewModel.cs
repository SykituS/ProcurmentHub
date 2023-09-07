using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrpcShared;
using ProcurementHub.Controls;
using ProcurementHub.Model.CustomModels;
using ProcurementHub.Services;
using ProcurementHub.ViewModel.BaseViewModels;

namespace ProcurementHub.ViewModel.Orders
{
    [QueryProperty(nameof(TeamModel), "TeamMainModel")]
    public partial class OrderListViewModel : BaseViewModel
    {
        public ObservableCollection<OrderModel> Orders { get; set; } = new();
        private OrderServices _orderServices;

        [ObservableProperty]
        private TeamMainModel _teamModel;

        public OrderListViewModel(Procurement.ProcurementClient procurementClient, OrderServices orderServices) : base(procurementClient)
        {
            _orderServices = orderServices;
        }

        [ObservableProperty]
        bool _isRefreshing;

        [RelayCommand]
        async Task GoToOrderDetails(OrderModel order)
        {

        }

        [RelayCommand]
        async Task LoadOrderList()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            IsRefreshing = true;

            try
            {
                var response = await _orderServices.GetOrderListForTeam(_teamModel.ID);

                if (response.Successful)
                {
                    if (Orders.Any())
                        Orders.Clear();

                    foreach (var value in response.ResultValues)
                        Orders.Add(value);
                    
                }
                else
                {
                    await SnackBarControl.CreateSnackBar(response.Information);
                }
            }
            finally
            {
                IsBusy = false;
                IsRefreshing = false;
            }
        }

        [RelayCommand]
        async Task GoBack()
        {

        }

        async partial void OnTeamModelChanged(TeamMainModel value)
        {
            await LoadOrderList();
        }
    }
}
