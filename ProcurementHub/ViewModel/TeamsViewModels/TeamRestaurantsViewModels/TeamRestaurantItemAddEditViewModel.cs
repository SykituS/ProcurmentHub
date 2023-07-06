using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrpcShared;

namespace ProcurementHub.ViewModel.TeamsViewModels.TeamRestaurantsViewModels
{
    class TeamRestaurantItemAddEditViewModel : BaseViewModel
	{
		public TeamRestaurantItemAddEditViewModel(Procurement.ProcurementClient procurementClient) : base(procurementClient)
		{
		}
	}
}
