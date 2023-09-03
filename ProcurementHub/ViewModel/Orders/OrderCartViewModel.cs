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
        public ObservableCollection<OrderItemsModel> OrderSelectedItems { get; set; } = new();
        private TeamRestaurantsService _teamRestaurantsService;
        private OrderServices _orderServices;

        [ObservableProperty]
        private string _itemListJson;

        [ObservableProperty]
        private TeamMainModel _teamModel;

        [ObservableProperty]
        private OrderModel _orderModel;

        public OrderCartViewModel(Procurement.ProcurementClient procurementClient, TeamRestaurantsService teamRestaurantsService, OrderServices orderServices) : base(procurementClient)
        {
	        _teamRestaurantsService = teamRestaurantsService;
	        _orderServices = orderServices;
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
                var list = JsonConvert.DeserializeObject<ObservableCollection<OrderItemsModel>>(_itemListJson);

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
        async Task UpdateItemList()
        {
	        if (IsBusy)
		        return;

	        IsBusy = true;
	        IsRefreshing = true;

	        try
	        {
		        var list = OrderSelectedItems.ToList();


				if (OrderSelectedItems.Count != 0)
			        OrderSelectedItems.Clear();
                
		        foreach (var item in list)
		        {
			        OrderSelectedItems.Add(item);
		        }
	        }
	        finally
	        {
		        IsBusy = false;
		        IsRefreshing = false;
	        }
        }

		[RelayCommand]
        async Task ManageSelectedItem(OrderItemsModel model)
        {
            activePopUp = OrderCartControl.GeneratePopupForItemManagement(RemoveItemFromCartCommand, SplitItemCommand, model);
            await App.Current.MainPage.ShowPopupAsync(activePopUp);
        }

        [RelayCommand]
        async Task SplitItem()
        {
            //TODO: Implement splitting of item
            await SnackBarControl.CreateSnackBar("Function not implemented yet");
            activePopUp.Close();
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
        void RemoveItemFromCart(OrderItemsModel model)
        {
            var item = OrderSelectedItems.First(e => e.TeamRestaurantsItemID == model.TeamRestaurantsItemID);
            
            item.Quantity--;
            item.TotalPriceOfItem -= item.TeamRestaurantsItemPrice;
            
            if (item.Quantity <= 0)
	            OrderSelectedItems.Remove(item);

            activePopUp.Close();
            IsRefreshing = true;
        }

        [RelayCommand]
        async Task ConfirmYourCart()
        {
            activePopUp = OrderCartControl.GeneratePopupForCartConfirmation(FinishOrderCommand);
            await App.Current.MainPage.ShowPopupAsync(activePopUp);
        }

        [RelayCommand]
        async Task FinishOrder(bool userWantToFinish)
        {
            if (!userWantToFinish)
            {
                activePopUp.Close();
                return;
            }

            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
	            await SnackBarControl.CreateSnackBar("Finishing your order");
	            var result = await _orderServices.AddItemsToOrder(OrderSelectedItems.ToList(), OrderModel.ID, true);

	            if (result.Successful)
	            {
					await Shell.Current.GoToAsync(nameof(OrderSelectItemsPage), true, new Dictionary<string, object>
					{
						{"TeamMainModel", _teamModel },
						{"OrderModel", _orderModel },
					});
				}
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
