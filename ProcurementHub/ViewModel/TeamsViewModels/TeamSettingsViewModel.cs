using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrpcShared;
using ProcurementHub.Model;
using ProcurementHub.Model.CustomModels;

namespace ProcurementHub.ViewModel.TeamsViewModels
{
	[QueryProperty(nameof(Model), "TeamSettingsModel")]
    public partial class TeamSettingsViewModel : BaseViewModels.BaseViewModel
    {
	    public TeamSettingsViewModel(Procurement.ProcurementClient procurementClient) : base(procurementClient)
	    {
	    }

		[ObservableProperty]
	    private TeamSettingsModel _model;
		
	}
}
