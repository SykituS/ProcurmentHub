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
		async Task CreateNewRestaurant()
		{

		}

		[RelayCommand]
		async Task GoBackToTeamRestaurants()
		{
			await Shell.Current.GoToAsync("..");
		}
	}
}
