using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

		}

		[RelayCommand]
		async Task GoBackToTeamRestaurantItems()
		{
			await Shell.Current.GoToAsync("..");
		}
	}
}
