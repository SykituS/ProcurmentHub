using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrpcShared;
using GrpcShared.Models;

namespace ProcurementHub.ViewModel.TeamsViewModels
{
    [QueryProperty(nameof(Team), "Teams")]
    public partial class TeamMainViewModel : BaseViewModel
    {
        public TeamMainViewModel(Procurement.ProcurementClient procurementClient) : base(procurementClient)
        {
        }

        [ObservableProperty]
        private Teams _team;

        [RelayCommand]
        async Task StartNewOrder()
        {

        }

        [RelayCommand]
        async Task GoToTeamSettingsPage()
        {

        }

        [RelayCommand]
        async Task GoToTeamMembersPage()
        {

        }

    }
}
