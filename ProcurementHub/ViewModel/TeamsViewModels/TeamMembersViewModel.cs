using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrpcShared;
using ProcurementHub.Model.CustomModels;
using ProcurementHub.Services;
using ProcurementHub.ViewModel.BaseViewModels;

namespace ProcurementHub.ViewModel.TeamsViewModels
{
    [QueryProperty(nameof(TeamModel), "TeamMainModel")]
    public partial class TeamMembersViewModel : BaseViewModel
    {
        private TeamsService _teamsService;
        public ObservableCollection<TeamMembersModel> TeamMembers { get; set; } = new();

        [ObservableProperty]
        private TeamMainModel _teamModel;

        public TeamMembersViewModel(Procurement.ProcurementClient procurementClient, TeamsService teamsService) : base(procurementClient)
        {
            _teamsService = teamsService;
        }

        [ObservableProperty]
        private bool _isRefreshing;

        [RelayCommand]
        async Task LoadTeamMembers()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var reply = await _teamsService.GetTeamMembers(TeamModel.ID);

                if (reply.Successful)
                {
                    if (TeamMembers.Any())
                        TeamMembers.Clear();


                    foreach (var member in reply.ResultValues)
                        TeamMembers.Add(member);
                }
            }
            finally
            {
                IsBusy = false;
                IsRefreshing = false;
            }
            
        }

        async partial void OnTeamModelChanged(TeamMainModel value)
        {
            await LoadTeamMembers();
        }
    }
}
