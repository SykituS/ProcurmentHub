using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrpcShared;
using ProcurementHub.Model.CustomModels;
using ProcurementHub.Model.Models;
using ProcurementHub.Services;

namespace ProcurementHub.ViewModel.TeamsViewModels.TeamRestaurantsViewModels
{
	[QueryProperty(nameof(Model), "TeamRestaurant")]
	public partial class TeamRestaurantItemsViewModel : BaseViewModels.BaseViewModel
	{
		public ObservableCollection<TeamRestaurantItemsModel> TeamRestaurantItems { get; set; } = new();
		private TeamRestaurantsService _teamRestaurantsService;

		[ObservableProperty]
		private TeamRestaurantsModel _model;

		public TeamRestaurantItemsViewModel(Procurement.ProcurementClient procurementClient, TeamRestaurantsService teamRestaurantsService) : base(procurementClient)
		{
			_teamRestaurantsService = teamRestaurantsService;
			GetRestaurantItems();
		}

		[ObservableProperty]
		bool _isRefreshing;

		[RelayCommand]
		async Task GetRestaurantItems()
		{
			Debug.WriteLine("Getting restaurant items");

		}

		[RelayCommand]
		async Task GoToEditRestaurantItem(TeamRestaurantItemsModel model)
		{
			Debug.WriteLine("going to edit page");

		}

		[RelayCommand]
		async Task GoToAddRestaurantItem()
		{
			Debug.WriteLine("going to add page");

		}
	}
}
