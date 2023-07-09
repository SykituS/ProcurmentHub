using GrpcShared;
using ProcurementHub.Model.CustomModels;

namespace ProcurementHub.ViewModel.AccountViewModels
{
    public partial class ForgotPasswordViewModel : BaseViewModels.BaseViewModel
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
