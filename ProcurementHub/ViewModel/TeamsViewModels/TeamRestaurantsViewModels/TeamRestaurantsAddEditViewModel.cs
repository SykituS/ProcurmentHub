using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using GrpcShared;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using ProcurementHub.Model.CustomModels;
using ProcurementHub.Services;
using ProcurementHub.View.Teams.TeamRestaurants;
using IMap = Microsoft.Maui.Maps.IMap;
using Map = Microsoft.Maui.Controls.Maps.Map;

namespace ProcurementHub.ViewModel.TeamsViewModels.TeamRestaurantsViewModels
{
	[QueryProperty(nameof(Model), "TeamMainModel")]
	[QueryProperty(nameof(TeamRestaurantsModel), "TeamRestaurant")]
	public partial class TeamRestaurantsAddEditViewModel : BaseViewModels.BaseViewModel
	{
		private TeamRestaurantsService _teamRestaurantsService;
		private ApiServices _apiServices;

		[ObservableProperty]
		private TeamMainModel _model;

		[ObservableProperty]
		private TeamRestaurantsModel _teamRestaurantsModel = new ();

		public TeamRestaurantsAddEditViewModel(Procurement.ProcurementClient procurementClient, TeamRestaurantsService teamRestaurantsService, ApiServices apiServices) : base(procurementClient)
        {
            _teamRestaurantsService = teamRestaurantsService;
            _apiServices = apiServices;
        }

		[RelayCommand]
		async Task CreateOrUpdateRestaurant()
		{
			if (IsBusy)
				return;

			IsBusy = true;
			try
			{
                var result = await _teamRestaurantsService.CreateOrUpdateRestaurantAsync(_teamRestaurantsModel, _model.ID);

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
		async Task GoBackToTeamRestaurants()
		{
			await Shell.Current.GoToAsync("..");
		}

		[RelayCommand]
        async Task MapClicked()
        {
            await _apiServices.ConvertAddressToGeoLocation();
        }
    }
}
