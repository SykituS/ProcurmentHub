using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using GrpcShared;
using ProcurementHub.Model.CustomModels;
using ProcurementHub.Services;
using ProcurementHub.View.Orders;
using ProcurementHub.View.Teams;
using ProcurementHub.ViewModel.BaseViewModels;

namespace ProcurementHub.ViewModel.Orders
{
	[QueryProperty(nameof(TeamModel), "TeamMainModel")]
	public partial class OrderStartViewModel : BaseViewModel
	{
		public ObservableCollection<TeamRestaurantsModel> RestaurantsForOrder { get; set; } = new();
		private readonly TeamRestaurantsService _teamRestaurantsService;
		private readonly OrderServices _orderServices;

		[ObservableProperty]
		private TeamMainModel _teamModel;

		public OrderStartViewModel(Procurement.ProcurementClient procurementClient, TeamRestaurantsService teamRestaurantsService, OrderServices orderServices) : base(procurementClient)
		{
			_teamRestaurantsService = teamRestaurantsService;
			_orderServices = orderServices;
		}

		[ObservableProperty]
		bool _isRefreshing;

		[RelayCommand]
		async Task GetRestaurant()
		{
			if (IsBusy)
				return;

			IsBusy = true;

			try
			{
				var result = await _teamRestaurantsService.GetRestaurantListAsync(_teamModel.ID);

				if (result.Successful)
				{
					if (RestaurantsForOrder.Count != 0)
						RestaurantsForOrder.Clear();

					foreach (var restaurant in result.ResultValues)
						RestaurantsForOrder.Add(restaurant);
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
		async Task StartOrderWithThisRestaurant(TeamRestaurantsModel model)
		{
			if (IsBusy)
				return;

			IsBusy = true;

			try
			{
				var result = await _orderServices.StartNewOrder(TeamModel.ID, model.ID);

				if (result.Successful)
				{
					result.ResultValues.Restaurants = model;
					await Shell.Current.GoToAsync(nameof(OrderSelectItemsPage), true, new Dictionary<string, object>
					{
						{"TeamRestaurant", _teamModel },
						{"OrderModel", result.ResultValues },
					});
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
			}
		}

		[RelayCommand]
		async Task GoBackToTeam()
		{
			await Shell.Current.GoToAsync(nameof(TeamMainPage), true, new Dictionary<string, object>
			{
				{"TeamMainModel", _teamModel }
			});
		}

		async partial void OnTeamModelChanged(TeamMainModel value)
		{
			await GetRestaurant();
		}
	}
}
