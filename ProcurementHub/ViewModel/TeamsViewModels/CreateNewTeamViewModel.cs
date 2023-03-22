using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using GrpcShared;
using GrpcShared.Models;
using ProcurementHub.Services;
using ProcurementHub.View.Main;

namespace ProcurementHub.ViewModel.TeamsViewModels
{
    public partial class CreateNewTeamViewModel : BaseViewModel
    {
        private TeamsService TeamsService;

        public CreateNewTeamViewModel(Procurement.ProcurementClient procurementClient, TeamsService teamsService) :
            base(procurementClient)
        {
            TeamsService = teamsService;
            Title = "Create new team";
        }

        [ObservableProperty] private Teams team = new();

        [RelayCommand]
        async Task GoBackToMainPage()
        {
            await Shell.Current.GoToAsync(nameof(MainPage), true);
        }

        [RelayCommand]
        async Task CreateNewTeam()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                var result = await TeamsService.CreateNewTeamAsync(team);

                if (result.Successful)
                {
                    await Shell.Current.DisplayAlert("Success", result.Information, "OK");
                    await Shell.Current.GoToAsync(nameof(MainPage), true);
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", result.Information, "OK");
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
    }
}