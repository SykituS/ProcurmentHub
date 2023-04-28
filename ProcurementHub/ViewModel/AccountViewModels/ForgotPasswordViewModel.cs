using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrpcShared;
using GrpcShared.Models;
using ProcurementHub.Model;

namespace ProcurementHub.ViewModel
{
    public partial class ForgotPasswordViewModel : BaseViewModel
    {
        public ForgotPasswordViewModel(Procurement.ProcurementClient procurementClient) : base(procurementClient)
        {
            Title = "Forgot password";
        }

        [ObservableProperty] private ForgotPasswordModel _forgotPassword = new();

        [RelayCommand]
        async Task SendRequestToResetPassword()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {

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
