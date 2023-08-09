using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrpcShared;
using Newtonsoft.Json;
using ProcurementHub.Model.CustomModels;
using ProcurementHub.Services;
using ProcurementHub.View.Orders;
using ProcurementHub.ViewModel.BaseViewModels;

namespace ProcurementHub.ViewModel.Orders
{
    [QueryProperty(nameof(TeamModel), "TeamMainModel")]
    [QueryProperty(nameof(OrderModel), "OrderModel")]
    [QueryProperty(nameof(ItemListJson), "OrderSelectedItems")]
    public partial class OrderCartViewModel : BaseViewModel
    {
        public ObservableCollection<TeamRestaurantItemsModel> OrderSelectedItems { get; set; } = new();
        private TeamRestaurantsService _teamRestaurantsService;

        [ObservableProperty]
        private string _itemListJson;

        [ObservableProperty]
        private TeamMainModel _teamModel;

        [ObservableProperty]
        private OrderModel _orderModel;
        
        public OrderCartViewModel(Procurement.ProcurementClient procurementClient, TeamRestaurantsService teamRestaurantsService) : base(procurementClient)
        {
            _teamRestaurantsService = teamRestaurantsService;
        }

        [ObservableProperty]
        bool _isRefreshing;

        [RelayCommand]
        async Task GetItemList()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            IsRefreshing = true;

            try
            {
                var list = JsonConvert.DeserializeObject<ObservableCollection<TeamRestaurantItemsModel>>(_itemListJson);

                if (OrderSelectedItems.Count != 0)
                    OrderSelectedItems.Clear();

                if (list != null)
                {
                    foreach (var item in list)
                    {
                        OrderSelectedItems.Add(item);
                    }

                }
            }
            finally
            {
                IsBusy = false;
                IsRefreshing = false;
            }

        }

        [RelayCommand]
        async Task ManageSelectedItem()
        {

        }

        [RelayCommand]
        async Task GoBackToSelectItem()
        {
            await Shell.Current.GoToAsync("..", true);
        }

        [RelayCommand]
        async Task ConfirmYourCart()
        {

        }

        async partial void OnItemListJsonChanged(string value)
        {
            await GetItemList();
        }
    }
}
