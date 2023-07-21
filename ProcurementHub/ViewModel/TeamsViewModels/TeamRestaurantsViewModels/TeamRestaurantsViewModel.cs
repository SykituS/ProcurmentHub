using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using GrpcShared;
using ProcurementHub.Model.CustomModels;
using ProcurementHub.Services;
using ProcurementHub.View.Teams.TeamRestaurants;

namespace ProcurementHub.ViewModel.TeamsViewModels.TeamRestaurantsViewModels
{
    [QueryProperty(nameof(Model), "TeamMainModel")]
    public partial class TeamRestaurantsViewModel : BaseViewModels.BaseViewModel
	{
		public ObservableCollection<TeamRestaurantsModel> TeamRestaurants { get; set; } = new();
		private readonly TeamRestaurantsService _teamRestaurantsService;

		[ObservableProperty]
		[NotifyCanExecuteChangedFor(nameof(GetRestaurantCommand))]
		private TeamMainModel _model;

		public TeamRestaurantsViewModel(Procurement.ProcurementClient procurementClient, TeamRestaurantsService teamRestaurantsService) : base(procurementClient)
		{
			_teamRestaurantsService = teamRestaurantsService;
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
				var result = await _teamRestaurantsService.GetRestaurantListAsync(_model.ID);

				if (result.Successful)
				{
					if (TeamRestaurants.Count != 0)
						TeamRestaurants.Clear();

					foreach (var restaurant in result.ResultValues)
						TeamRestaurants.Add(restaurant);
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
		async Task GoToRestaurant(TeamRestaurantsModel model)
		{
			await Shell.Current.GoToAsync(nameof(TeamRestaurantItemsPage), true, new Dictionary<string, object>
			{
				{"TeamRestaurant", model }
			});

			//Open edit page with model of restaurant
			//await Shell.Current.GoToAsync(nameof(TeamRestaurantsAddEditPage), true, new Dictionary<string, object>
			//{
			//	{"TeamMainModel", _model },
			//	{"TeamRestaurant", model }
			//});
		}

		[RelayCommand]
		async Task GoToAddNewRestaurant()
		{
			Debug.WriteLine("going to add new restaurant");
			await Shell.Current.GoToAsync(nameof(TeamRestaurantsAddEditPage), true, new Dictionary<string, object>
			{
				{"TeamMainModel", _model },
				{"TeamRestaurant", new TeamRestaurantsModel() }
			});
		}

		async partial void OnModelChanged(TeamMainModel value)
		{
			await GetRestaurant();
		}
	}
}
