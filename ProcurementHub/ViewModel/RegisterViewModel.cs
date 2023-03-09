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

namespace ProcurementHub.ViewModel
{
    public partial class RegisterViewModel : BaseViewModel
    {
        public RegisterViewModel(Procurement.ProcurementClient procurementClient) : base(procurementClient)
        {
            Title = "Register";
        }

        [ObservableProperty] private RegisterNewUserModel _registerNewUser = new() {Person = new Persons()};

        [ObservableProperty]
        private string _errorMessage;

        [RelayCommand]
        async Task Register()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var result = await new RegisterServices(ProcurementClient).RegisterNewUserAsync(_registerNewUser);

                if (result.Successful)
                {
                    await Shell.Current.DisplayAlert("Success!", "Account was created", "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Error occurred: " + result.Information, "OK");
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
            }
        }

        [RelayCommand]
        async Task GoToLoginPage()
        {
            await Shell.Current.GoToAsync("..", true);
        }
    }
}
