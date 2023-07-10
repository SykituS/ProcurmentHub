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
    public partial class TeamRestaurantsViewModel : BaseViewModels.BaseViewModel
	{
		public ObservableCollection<TeamRestaurantsModel> TeamRestaurants { get; set; } = new();
		private TeamRestaurantsService _teamRestaurantsService;

		[ObservableProperty]
		private TeamMainModel _model;

		public TeamRestaurantsViewModel(Procurement.ProcurementClient procurementClient, TeamRestaurantsService teamRestaurantsService) : base(procurementClient)
		{
			_teamRestaurantsService = teamRestaurantsService;
			GetRestaurant();
		}

		[ObservableProperty]
		bool _isRefreshing;

		[RelayCommand]
		async Task GetRestaurant()
		{
			Debug.WriteLine("Getting restaurants");
			var restaurant = new TeamRestaurantsModel
			{
				ID = 2,
				Name = "McDonald",
				Address = "Jan Paweł II 23/5",
				Description = "",
				CreatedOn = "23, June 2023",
				CreatedBy = new PersonsModel() {FullName = "Jan Testowy"},
				IsUpdated = true,
				UpdatedOn = "25, June 2023",
				UpdatedBy = new PersonsModel() {FullName = "Test Test"}
			};
			TeamRestaurants.Add(restaurant);
		}

		[RelayCommand]
		async Task GoToRestaurant(TeamRestaurantsModel model)
		{
			Debug.WriteLine("Changing page to team restaurant items");
		}
	}
}
