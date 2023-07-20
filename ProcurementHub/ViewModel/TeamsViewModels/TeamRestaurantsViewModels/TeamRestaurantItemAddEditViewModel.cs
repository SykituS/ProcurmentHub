using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using GrpcShared;
using ProcurementHub.Model.CustomModels;
using ProcurementHub.Services;

namespace ProcurementHub.ViewModel.TeamsViewModels.TeamRestaurantsViewModels
{
	[QueryProperty(nameof(RestaurantModel), "TeamRestaurant")]
	[QueryProperty(nameof(TeamRestaurantItemsModel), "TeamRestaurantItem")]
	public partial class TeamRestaurantItemAddEditViewModel : BaseViewModels.BaseViewModel
	{
		private TeamRestaurantsService _teamRestaurantsService;

		[ObservableProperty]
		private TeamRestaurantsModel _restaurantModel;

		[ObservableProperty]
		private TeamRestaurantItemsModel _teamRestaurantItemsModel;

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
                var result = await _teamRestaurantsService.Crea(_teamRestaurantsModel, _model.ID);

                if (result.Successful)
                {
                    await Shell.Current.DisplayAlert("Success", result.Information, "OK");
                    await Shell.Current.GoToAsync(nameof(TeamRestaurantsPage), true, new Dictionary<string, object>
                    {
                        {"TeamMainModel", _model }
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
	}
}
