using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrpcShared;
using ProcurementHub.View.Account;

namespace ProcurementHub.ViewModel
{
    public partial class AppShellViewModel : BaseViewModel
    {
        public AppShellViewModel(Procurement.ProcurementClient procurementClient) : base(procurementClient)
        {
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
    }
}
