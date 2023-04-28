using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrpcShared;
using ProcurementHub.View.Account;
using ProcurementHub.View.Main;
using ProcurementHub.View.Teams;

namespace ProcurementHub.ViewModel.AccountViewModels
{
    public partial class ProfileManagementViewModel : BaseViewModel
    {
        public ProfileManagementViewModel(Procurement.ProcurementClient procurementClient) : base(procurementClient)
        {
            Title = "Manage your profile!";
        }

        [RelayCommand]
        async void SignOut()
        {
            if (Preferences.ContainsKey(nameof(App.LoggedUserInApplication)))
            {
                Preferences.Remove(nameof(App.LoggedUserInApplication));
            }
            App.LoggedUserInApplication = null;
            await Shell.Current.GoToAsync($"{nameof(LoginPage)}");
        }

        [RelayCommand]
        async Task GoToDashboard()
        {
            await Shell.Current.GoToAsync(nameof(MainPage), true);
        }

        [RelayCommand]
        async Task GoToCreateNewTeam()
        {
            await Shell.Current.GoToAsync(nameof(CreateNewTeamPage), true);
        }

        [RelayCommand]
        async Task GoToJoinTeam()
        {
            await Shell.Current.GoToAsync(nameof(JoinTeamPage), true);
        }
    }
}
