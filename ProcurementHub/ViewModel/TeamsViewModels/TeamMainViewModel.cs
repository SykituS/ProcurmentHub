using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using GrpcShared;
using GrpcShared.Models;
using ProcurementHub.Model;
using ProcurementHub.View.Teams;

namespace ProcurementHub.ViewModel.TeamsViewModels
{
    [QueryProperty(nameof(Model), "TeamMainModel")]
    public partial class TeamMainViewModel : BaseViewModel
    {
        public TeamMainViewModel(Procurement.ProcurementClient procurementClient) : base(procurementClient)
        {
        }

        [ObservableProperty]
        private TeamMainModel _model;

        [RelayCommand]
        async Task StartNewOrder()
        {
            Debug.WriteLine("Starting new order");
        }

        [RelayCommand]
        async Task GoToTeamSettingsPage()
        {
	        if (IsBusy)
		        return;

	        IsBusy = true;

	        try
	        {
		        //var result = await TeamsService.GetSelectedTeam(_model.ID);

		        //if (result.Successful)
		        //{
		        var teamSettingsModel = new TeamSettingsModel
		        {
			        ID = _model.ID,
			        Description = _model.Description,
			        Status = _model.Status,
			        TeamJoinCode = "null",
			        TeamJoinPassword = "null"
		        };

		        await Shell.Current.GoToAsync(nameof(TeamSettingsPage), true, new Dictionary<string, object>
		        {
			        {"TeamSettingsModel", teamSettingsModel }
		        });
		        //}
		        //else
		        //{
		        //	await Shell.Current.DisplayAlert("Error", result.Information, "OK");
		        //}
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
        async Task GoToTeamMembersPage()
        {
	        Debug.WriteLine("Changing page to team members page");

		}

		[RelayCommand]
        async Task GoToTeamStatisticsPage()
        {
	        Debug.WriteLine("Changing page to team statistics page");

		}
	}
}
