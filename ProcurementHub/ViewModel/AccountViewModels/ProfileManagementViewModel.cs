using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrpcShared;
using ProcurementHub.Model;
using ProcurementHub.Model.CustomModels;
using ProcurementHub.View.Account;
using ProcurementHub.View.Main;
using ProcurementHub.View.Teams;

namespace ProcurementHub.ViewModel.AccountViewModels
{
    public partial class ProfileManagementViewModel : BaseViewModels.BaseViewModel
    {
        public ProfileManagementViewModel(Procurement.ProcurementClient procurementClient) : base(procurementClient)
        {
            Title = "Manage your profile!";
            _model.ProfileWelcomeText = $"Hi, {App.LoggedUserInApplication.Person.GetFullName()}";
        }

        [ObservableProperty] private ProfileManagementModel _model = new ProfileManagementModel();

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
