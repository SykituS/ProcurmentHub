using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
				ID = 1,
				Name = "McDonald",
				Address = "Jan Paweł II 23/5",
				Description = "",
				CreatedOn = "23, June 2023",
				CreatedBy = new PersonsModel() {FullName = "Jan Testowy"},
				IsUpdated = true,
				UpdatedOn = "25, June 2023",
				UpdatedBy = new PersonsModel() {FullName = "Test Test"}
			};
			var restaurant2 = new TeamRestaurantsModel
			{
				ID = 2,
				Name = "Sushi",
				Address = "Rewolucji 1939",
				Description = "Test",
				CreatedOn = "15, June 2023",
				CreatedBy = new PersonsModel() { FullName = "Adam Borowicz" },
				IsUpdated = false,
				UpdatedOn = "25, June 2023",
				UpdatedBy = new PersonsModel() { FullName = "Test Test" }
			};
			var restaurant3 = new TeamRestaurantsModel
			{
				ID = 3,
				Name = "McDonald",
				Address = "Jan Paweł II 23/5",
				Description = "",
				CreatedOn = "23, June 2023",
				CreatedBy = new PersonsModel() { FullName = "Jan Testowy" },
				IsUpdated = true,
				UpdatedOn = "25, June 2023",
				UpdatedBy = new PersonsModel() { FullName = "Test Test" }
			};
			TeamRestaurants.Add(restaurant);
			TeamRestaurants.Add(restaurant2);
			TeamRestaurants.Add(restaurant3);
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
	}
}
