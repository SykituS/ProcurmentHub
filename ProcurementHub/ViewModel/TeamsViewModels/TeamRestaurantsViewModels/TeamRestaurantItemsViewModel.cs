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
    public partial class TeamRestaurantItemsViewModel : BaseViewModels.BaseViewModel
	{
		public TeamRestaurantItemsViewModel(Procurement.ProcurementClient procurementClient) : base(procurementClient)
		{
		}
	}
}
