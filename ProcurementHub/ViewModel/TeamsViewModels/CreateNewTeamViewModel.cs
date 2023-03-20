using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrpcShared;
using GrpcShared.Models;

namespace ProcurementHub.ViewModel.TeamsViewModels
{
    public partial class CreateNewTeamViewModel : BaseViewModel
    {
        public CreateNewTeamViewModel(Procurement.ProcurementClient procurementClient) : base(procurementClient)
        {
            Title = "Create new team";
        }

        [ObservableProperty] private Teams team;
    }
}
