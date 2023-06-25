using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrpcShared;
using GrpcShared.Models;
using ProcurementHub.Model;

namespace ProcurementHub.ViewModel.TeamsViewModels
{
    [QueryProperty(nameof(Model), "TeamMainModel")]
    public partial class TeamMainViewModel : BaseViewModel
    {
        public TeamMainViewModel(Procurement.ProcurementClient procurementClient) : base(procurementClient)
        {
        }

        [ObservableProperty]
        private TeamMainModel _model;

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
