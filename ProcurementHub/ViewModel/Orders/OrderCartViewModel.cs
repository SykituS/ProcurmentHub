using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using GrpcShared;
using Newtonsoft.Json;
using ProcurementHub.Controls;
using ProcurementHub.Model.CustomModels;
using ProcurementHub.Services;
using ProcurementHub.View.Orders;
using ProcurementHub.ViewModel.BaseViewModels;
using Font = Microsoft.Maui.Font;

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

        private Popup activePopup;

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
        async Task ManageSelectedItem(TeamRestaurantItemsModel model)
        {
            activePopup = OrderCartPopups.GeneratePopupForItemManagement(RemoveItemFromCartCommand, SplitItemCommand, model);
            await App.Current.MainPage.ShowPopupAsync(activePopup);
        }

        [RelayCommand]
        async Task SplitItem()
        {
            //TODO: Implement splitting of item
            var cancellationTokenSource = new CancellationTokenSource();

            var snackbarOptions = new SnackbarOptions()
            {
                BackgroundColor = Colors.AliceBlue,
                TextColor = Colors.Black,
                CornerRadius = new CornerRadius(15),
                Font = Font.SystemFontOfSize(14),
            };

            var text = "Splitting of item is not implemented yet!";
            var duration = TimeSpan.FromSeconds(3);

            var snackbar = Snackbar.Make(text, null, duration: duration, visualOptions: snackbarOptions);
            
            await snackbar.Show(cancellationTokenSource.Token);
            activePopup.Close();
        }

        [RelayCommand]
        async Task GoBackToSelectItem()
        {
            var json = JsonConvert.SerializeObject(OrderSelectedItems);
            await Shell.Current.GoToAsync(nameof(OrderSelectItemsPage), true, new Dictionary<string, object>
            {
                {"TeamMainModel", _teamModel },
                {"OrderModel", _orderModel },
                {"OrderSelectedItems", json },
            });
        }

        [RelayCommand]
        void RemoveItemFromCart(TeamRestaurantItemsModel model)
        {
            OrderSelectedItems.Remove(model);
            activePopup.Close();
        }

        [RelayCommand]
        async Task ConfirmYourCart()
        {
            activePopup = OrderCartPopups.GeneratePopupForCartConfirmation(FinishOrderCommand);
            await App.Current.MainPage.ShowPopupAsync(activePopup);
        }

        [RelayCommand]
        async Task FinishOrder(bool userWantToFinish)
        {
            if (!userWantToFinish)
            {
                activePopup.Close();
                return;
            }

            if (IsBusy)
                return;

            IsBusy = true;

            try
            {

            }
            finally
            {
                IsBusy = false;
            }
        }

        async partial void OnItemListJsonChanged(string value)
        {
            await GetItemList();
        }
    }
}
