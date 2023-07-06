using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrpcShared;

namespace ProcurementHub.ViewModel.TeamsViewModels.TeamRestaurantsViewModels
{
    class TeamRestaurantsAddEditViewModel : BaseViewModel
	{
		public TeamRestaurantsAddEditViewModel(Procurement.ProcurementClient procurementClient) : base(procurementClient)
		{
		}
	}
}
