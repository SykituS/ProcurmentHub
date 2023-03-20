using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrpcShared;
using GrpcShared.Models;

namespace ProcurementHub.ViewModel.TeamsViewModels
{
    public partial class JoinTeamViewModel : BaseViewModel
    {
        public JoinTeamViewModel(Procurement.ProcurementClient procurementClient) : base(procurementClient)
        {
            Title = "Join existing team";
        }

        [ObservableProperty] private Teams team;

    }
}
