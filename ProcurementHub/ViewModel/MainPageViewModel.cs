﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using GrpcShared;
using GrpcShared.Models;
using ProcurementHub.Services;

namespace ProcurementHub.ViewModel
{
    public partial class MainPageViewModel : BaseViewModel
    {
        public ObservableCollection<Teams> Teams { get; set; } = new();
        private TeamsService TeamsService;

        public MainPageViewModel(Procurement.ProcurementClient procurementClient, TeamsService teamsService) : base(procurementClient)
        {
            TeamsService = teamsService;
            GetTeamsAsync();
            Title = "Main Page";
        }
        
        async Task GetTeamsAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var result = await TeamsService.GetTeamListAsync();

                if (result.Successful)
                {
                    if (Teams.Count != 0)
                        Teams.Clear();
                    

                    foreach (var team in result.ResultValues)
                        Teams.Add(team);
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

        [RelayCommand]
        async Task GoToTeam()
        {

        }
    }
}
