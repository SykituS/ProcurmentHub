using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrpcShared;
using GrpcShared.Models;
using ProcurementHub.View.Main;

namespace ProcurementHub.ViewModel
{
    public partial class LoginViewModel : BaseViewModel
    {
        public LoginViewModel(Procurement.ProcurementClient procurementClient) : base(procurementClient)
        {
        }

        [ObservableProperty] 
        private Users _user;

        [ObservableProperty]
        private string _errorMessage;



        [RelayCommand]
        async Task Login()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                _errorMessage = "Test";

                await Shell.Current.GoToAsync(nameof(MainPage), true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }

        }
    }
}
