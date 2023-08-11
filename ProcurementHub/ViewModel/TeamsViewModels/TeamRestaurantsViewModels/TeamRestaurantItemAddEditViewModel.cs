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
	[QueryProperty(nameof(RestaurantModel), "TeamRestaurant")]
	[QueryProperty(nameof(TeamRestaurantItemsModel), "TeamRestaurantItem")]
	[QueryProperty(nameof(TeamModel), "TeamMainModel")]
	public partial class TeamRestaurantItemAddEditViewModel : BaseViewModels.BaseViewModel
	{
		private TeamRestaurantsService _teamRestaurantsService;

		[ObservableProperty]
		private TeamRestaurantsModel _restaurantModel;

		[ObservableProperty]
		private TeamRestaurantItemsModel _teamRestaurantItemsModel;

        [ObservableProperty]
        private TeamMainModel _teamModel;

        public TeamRestaurantItemAddEditViewModel(Procurement.ProcurementClient procurementClient, TeamRestaurantsService teamRestaurantsService) : base(procurementClient)
		{
			_teamRestaurantsService = teamRestaurantsService;
		}

		[RelayCommand]
		async Task CreateNewRestaurantItem()
		{
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                var result = await _teamRestaurantsService.CreateOrUpdateRestaurantItemAsync(_teamRestaurantItemsModel, _restaurantModel.ID);

                if (result.Successful)
                {
                    await Shell.Current.DisplayAlert("Success", result.Information, "OK");
                    await Shell.Current.GoToAsync(nameof(TeamRestaurantItemsPage), true, new Dictionary<string, object>
                    {
                        {"TeamRestaurant", _restaurantModel },
                        {"TeamMainModel", _teamModel }
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
		async Task GoBackToTeamRestaurantItems()
		{
			await Shell.Current.GoToAsync("..");
		}

		partial void OnTeamRestaurantItemsModelChanging(TeamRestaurantItemsModel value)
		{
			value.CurrencyType = "PLN";
		}
	}
}
