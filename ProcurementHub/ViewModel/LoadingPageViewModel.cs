using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using GrpcShared.Models;
using Newtonsoft.Json;
using ProcurementHub.Controls;
using ProcurementHub.View.Account;
using ProcurementHub.View.Main;

namespace ProcurementHub.ViewModel
{
    public partial class LoadingPageViewModel
    {
        public LoadingPageViewModel()
        {
            CheckUserLoginDetails();
        }

        private async void CheckUserLoginDetails()
        {
            string userStr = Preferences.Get(nameof(App.User), "");

            if (string.IsNullOrWhiteSpace(userStr))
            {
                // Navigate to Login page
                await Shell.Current.GoToAsync($"{nameof(LoginPage)}");
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<Users>(userStr);
                App.User = userInfo;
                Shell.Current.FlyoutHeader = new FlyoutHeaderControl();

                //Navigate to Main Page
                await Shell.Current.GoToAsync($"{nameof(MainPage)}");
            }
        }
    }
}
