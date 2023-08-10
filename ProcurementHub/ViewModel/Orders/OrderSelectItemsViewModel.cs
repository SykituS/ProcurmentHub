using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Grpc.Core;
using GrpcShared;
using Newtonsoft.Json;
using ProcurementHub.Controls;
using ProcurementHub.Model.CustomModels;
using ProcurementHub.Services;
using ProcurementHub.View.Orders;
using ProcurementHub.View.Teams;
using ProcurementHub.ViewModel.BaseViewModels;
using Font = Microsoft.Maui.Font;

namespace ProcurementHub.ViewModel.Orders
{
	[QueryProperty(nameof(TeamModel), "TeamMainModel")]
	[QueryProperty(nameof(OrderModel), "OrderModel")]
    [QueryProperty(nameof(ItemListJson), "OrderSelectedItems")]
	public partial class OrderSelectItemsViewModel : BaseViewModel
	{
		public ObservableCollection<TeamRestaurantItemsModel> RestaurantsItems { get; set; } = new();
        public List<TeamRestaurantItemsModel> OrderSelectedItems { get; set; } = new();
		private TeamRestaurantsService _teamRestaurantsService;

        [ObservableProperty]
        private string _itemListJson;

        [ObservableProperty]
		private TeamMainModel _teamModel;

		[ObservableProperty]
		private OrderModel _orderModel;

		public OrderSelectItemsViewModel(Procurement.ProcurementClient procurementClient, TeamRestaurantsService teamRestaurantsService) : base(procurementClient)
		{
			_teamRestaurantsService = teamRestaurantsService;
		}

		[ObservableProperty]
		bool _isRefreshing;

		[RelayCommand]
		async Task GetRestaurantItems()
		{
			if (IsBusy)
				return;

			IsBusy = true;

			try
			{
				var result = await _teamRestaurantsService.GetRestaurantItemsListAsync(_orderModel.TeamRestaurantID);

				if (result.Successful)
				{
					if (RestaurantsItems.Count != 0)
						RestaurantsItems.Clear();

					foreach (var item in result.ResultValues)
						RestaurantsItems.Add(item);
				}
				else
				{
					await Shell.Current.DisplayAlert("Error", result.Information, "OK");
				}
			}
			catch (RpcException ex)
			{
				await Shell.Current.DisplayAlert("Error", "Error while connecting to the server", "OK");
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
				await Shell.Current.DisplayAlert("Error", "Unexpected error occurred! Please try again!", "OK");
			}
			finally
			{
				IsBusy = false;
				IsRefreshing = false;
			}
		}
		
		[RelayCommand]
		async Task AddItemToCart(TeamRestaurantItemsModel item)
		{
            OrderSelectedItems.Add(item);
            await SnackBarControl.CreateSnackBar("New item was added to cart");
        }

        [RelayCommand]
        async Task LeaveOrder()
        {
            await Shell.Current.GoToAsync(nameof(TeamMainPage), true, new Dictionary<string, object>
            {
                {"TeamMainModel", _teamModel }
            });
        }

        [RelayCommand]
		async Task GoToCart()
        {
            var json = JsonConvert.SerializeObject(OrderSelectedItems);
            await Shell.Current.GoToAsync(nameof(OrderCartPage), true, new Dictionary<string, object>
            {
                {"TeamMainModel", _teamModel },
                {"OrderModel", _orderModel },
                {"OrderSelectedItems", json },
            });
        }

        async partial void OnOrderModelChanged(OrderModel value)
		{
			await GetRestaurantItems();
		}

        async partial void OnItemListJsonChanged(string value)
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
    }
}
