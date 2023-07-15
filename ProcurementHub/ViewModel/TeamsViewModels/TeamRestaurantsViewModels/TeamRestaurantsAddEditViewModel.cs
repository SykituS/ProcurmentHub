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
	[QueryProperty(nameof(Model), "TeamMainModel")]
	[QueryProperty(nameof(TeamRestaurantsModel), "TeamRestaurant")]
	public partial class TeamRestaurantsAddEditViewModel : BaseViewModels.BaseViewModel
	{
		private TeamRestaurantsService _teamRestaurantsService;

		[ObservableProperty]
		private TeamMainModel _model;

		[ObservableProperty]
		private TeamRestaurantsModel _teamRestaurantsModel = new ();

		public TeamRestaurantsAddEditViewModel(Procurement.ProcurementClient procurementClient, TeamRestaurantsService teamRestaurantsService) : base(procurementClient)
		{
			_teamRestaurantsService = teamRestaurantsService;
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
	}
}
