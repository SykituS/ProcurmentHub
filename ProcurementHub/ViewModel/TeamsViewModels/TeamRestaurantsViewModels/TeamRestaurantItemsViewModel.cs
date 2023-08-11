using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using GrpcShared;
using ProcurementHub.Model.CustomModels;
using ProcurementHub.Model.Models;
using ProcurementHub.Services;
using ProcurementHub.View.Teams.TeamRestaurants;

namespace ProcurementHub.ViewModel.TeamsViewModels.TeamRestaurantsViewModels
{
	[QueryProperty(nameof(RestaurantModel), "TeamRestaurant")]
	[QueryProperty(nameof(TeamModel), "TeamMainModel")]
	public partial class TeamRestaurantItemsViewModel : BaseViewModels.BaseViewModel
	{
		public ObservableCollection<TeamRestaurantItemsModel> TeamRestaurantItems { get; set; } = new();
		private TeamRestaurantsService _teamRestaurantsService;

		[ObservableProperty]
		private TeamRestaurantsModel _restaurantModel;

        [ObservableProperty]
        private TeamMainModel _teamModel;

        public TeamRestaurantItemsViewModel(Procurement.ProcurementClient procurementClient, TeamRestaurantsService teamRestaurantsService) : base(procurementClient)
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
                var result = await _teamRestaurantsService.GetRestaurantItemsListAsync(_restaurantModel.ID);

                if (result.Successful)
                {
                    if (TeamRestaurantItems.Count != 0)
	                    TeamRestaurantItems.Clear();

                    foreach (var item in result.ResultValues)
	                    TeamRestaurantItems.Add(item);
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
        async Task GoBack()
        {
            await Shell.Current.GoToAsync(nameof(TeamRestaurantsPage), true, new Dictionary<string, object>
            {
                {"TeamMainModel", _teamModel }
            });
        }

        [RelayCommand]
		async Task GoToEditRestaurantItem(TeamRestaurantItemsModel model)
		{
			await Shell.Current.GoToAsync(nameof(TeamRestaurantItemAddEditPage), true, new Dictionary<string, object>
			{
				{"TeamRestaurant", _restaurantModel },
				{"TeamRestaurantItem", model },
                {"TeamMainModel", _teamModel }
            });
		}

		[RelayCommand]
		async Task GoToAddRestaurantItem()
		{
			await Shell.Current.GoToAsync(nameof(TeamRestaurantItemAddEditPage), true, new Dictionary<string, object>
			{
				{"TeamRestaurant", _restaurantModel },
				{"TeamRestaurantItem", new TeamRestaurantItemsModel() },
                {"TeamMainModel", _teamModel }
            });
		}

        async partial void OnRestaurantModelChanged(TeamRestaurantsModel value)
        {
            await GetRestaurantItems();
        }
    }
}
