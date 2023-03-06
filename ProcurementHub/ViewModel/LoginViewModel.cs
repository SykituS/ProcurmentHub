using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using GrpcShared;
using GrpcShared.Models;
using ProcurementHub.Model;
using ProcurementHub.Services;
using ProcurementHub.View.Account;
using ProcurementHub.View.Main;

namespace ProcurementHub.ViewModel
{
    public partial class LoginViewModel : BaseViewModel
    {
        public LoginViewModel(Procurement.ProcurementClient procurementClient) : base(procurementClient)
        {
        }

        [ObservableProperty] 
        private LoginUser _loginUser = new LoginUser();

        [ObservableProperty]
        private string _errorMessage;

        [ObservableProperty] 
        private bool _isRefreshing;

        [RelayCommand]
        async Task Login()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var result = await new LoginService(ProcurementClient).LoginUserAsync(_loginUser);
                if (result.Code != (int)HttpStatusCode.OK)
                {
                    await Shell.Current.DisplayAlert("Error", result.Message, "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert("ID", result.Message, "OK");

                    //await Shell.Current.GoToAsync(nameof(ForgotPasswordPage), true, new Dictionary<string, object>()
                    //{
                    //    {"Users", result}
                    //});
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error", "Unexpected error occurred! Please try again!", "OK");
            }
            finally
            {
                IsBusy = false;
                IsRefreshing = false;
            }
        }

        [RelayCommand]
        async Task GoToRegisterPage()
        {
            await Shell.Current.GoToAsync(nameof(RegisterPage), true, new Dictionary<string, object>
            {
                {"RegisterNewUserModel", new RegisterNewUserModel() {Person = new Persons()}}
            });
        }

        [RelayCommand]
        async Task GoToForgotPasswordPage()
        {
            await Shell.Current.GoToAsync(nameof(ForgotPasswordPage), true);
        }
    }
}
