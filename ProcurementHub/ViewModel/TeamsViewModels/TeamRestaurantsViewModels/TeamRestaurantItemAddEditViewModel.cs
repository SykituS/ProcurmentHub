using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrpcShared;

namespace ProcurementHub.ViewModel.TeamsViewModels.TeamRestaurantsViewModels
{
	public partial class TeamRestaurantItemAddEditViewModel : BaseViewModels.BaseViewModel
	{
		public TeamRestaurantItemAddEditViewModel(Procurement.ProcurementClient procurementClient) : base(procurementClient)
		{
		}
	}
}
