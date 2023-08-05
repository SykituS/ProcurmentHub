using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using GrpcShared;
using ProcurementHub.Model.CustomModels;
using ProcurementHub.Services;
using ProcurementHub.ViewModel.BaseViewModels;

namespace ProcurementHub.ViewModel.Orders
{
	[QueryProperty(nameof(TeamModel), "TeamModel")]
	[QueryProperty(nameof(OrderModel), "OrderModel")]
	public partial class OrderSelectItemsViewModel : BaseViewModel
	{
		public ObservableCollection<TeamRestaurantItemsModel> RestaurantsItems { get; set; } = new();
		private TeamRestaurantsService _teamRestaurantsService;

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

		//TODO: Showing items for selected restaurant
		//TODO: Adding items to cart
		//TODO: Move user to sum up page
		//TODO: Add possibility to remove items from cart
		[RelayCommand]
		async Task AddItemToCart()
		{

		}

		[RelayCommand]
		async Task GoToCart()
		{

		}

		async partial void OnOrderModelChanged(OrderModel value)
		{
			await GetRestaurantItems();
		}
	}
}
