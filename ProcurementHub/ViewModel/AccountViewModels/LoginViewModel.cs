using Grpc.Core;
using GrpcShared;
using Newtonsoft.Json;
using ProcurementHub.Controls;
using ProcurementHub.Model.CustomModels;
using ProcurementHub.Model.Models;
using ProcurementHub.Services;
using ProcurementHub.View.Account;
using ProcurementHub.View.Main;

namespace ProcurementHub.ViewModel.AccountViewModels
{
    public partial class LoginViewModel : BaseViewModels.BaseViewModel
    {
        private LoginService _loginService;
        public LoginViewModel(Procurement.ProcurementClient procurementClient, LoginService loginService) : base(procurementClient)
        {
            _loginService = loginService;
        }

        [ObservableProperty] 
        private LoginUser _loginUser = new();

        [ObservableProperty]
        private string _errorMessage;

        [ObservableProperty] 
        private bool _isRefreshing;

        [RelayCommand]
        async Task Login()
        {
            if (IsBusy)
                return;

            if (string.IsNullOrWhiteSpace(_loginUser.UserName) || string.IsNullOrWhiteSpace(_loginUser.Password))
            {
                    await Shell.Current.DisplayAlert("Error", "Please enter your credentials!", "OK");
                    return;
            }

            IsBusy = true;

            try
            {
                var result = await _loginService.LoginUserAsync(_loginUser);

                if (!result.ValidationResponse.Successful)
                {
                    await Shell.Current.DisplayAlert("Error", result.ValidationResponse.Information, "OK");
                }
                else
                {
                    var userData = await _loginService.ConvertRequestToUserData(result);

                    if (Preferences.ContainsKey(nameof(App.LoggedUserInApplication)))
                    {
                        Preferences.Remove(nameof(App.LoggedUserInApplication));
                    }

                    string userStr = JsonConvert.SerializeObject(userData);
                    Preferences.Set(nameof(App.LoggedUserInApplication), userStr);

                    App.LoggedUserInApplication = userData;
                    Shell.Current.FlyoutHeader = new FlyoutHeaderControl();

                    await Shell.Current.GoToAsync(nameof(MainPage), true);
                }
            }
            catch (RpcException ex)
            {
                await Shell.Current.DisplayAlert("Error", "Error while connecting to the server", "OK");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error", "Unexpected error occurred! Please try again!", "OK");
            }
            finally
            {
                IsBusy = false;
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
