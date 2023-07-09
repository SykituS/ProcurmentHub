using Newtonsoft.Json;
using ProcurementHub.Model.Models;
using ProcurementHub.View.Account;
using ProcurementHub.View.Main;

namespace ProcurementHub.ViewModel.MainViewModels
{
    public partial class LoadingPageViewModel
    {
        IConnectivity _connectivity;

        public LoadingPageViewModel(IConnectivity connectivity)
        {
            this._connectivity = connectivity;
            CheckInternetConnection();
            CheckUserLoginDetails();
            CheckConnectionTogRPC();
        }

        private async void CheckUserLoginDetails()
        {
            string userStr = Preferences.Get(nameof(App.LoggedUserInApplication), "");

            if (string.IsNullOrWhiteSpace(userStr))
            {
                // Navigate to Login page
                await Shell.Current.GoToAsync($"{nameof(LoginPage)}");
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<Users>(userStr);
                App.LoggedUserInApplication = userInfo;

                //Navigate to Main Page
                await Shell.Current.GoToAsync($"{nameof(MainPage)}");
            }
        }

        private async void CheckInternetConnection()
        {
            while (_connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                await Shell.Current.DisplayAlert("No connectivity!",
                    $"Please check internet and try again.", "OK");
            }
        }

        private async void CheckConnectionTogRPC()
        {

        }

    }
}
