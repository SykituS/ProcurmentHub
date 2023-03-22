using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrpcShared;
using GrpcShared.Models;
using ProcurementHub.Model;

namespace ProcurementHub.Services
{
    public class TeamsService : BaseServices
    {
        public TeamsService(Procurement.ProcurementClient procurementClient) : base(procurementClient)
        {
        }

        private List<Teams> _teams = new();

        public async Task<ValidationResponseWithResult<List<Teams>>> GetTeamListAsync()
        {
            var reply = await ProcurementClient.GetTeamsListAsync(new GRPCLoginInformationForUser
            {
                Id = App.LoggedUserInApplication.Id.ToString(),
                Username = App.LoggedUserInApplication.UserName,
                Password = App.LoggedUserInApplication.PasswordHash
            });

            var result = new ValidationResponseWithResult<List<Teams>>();

            if (!reply.ValidationResponse.Successful)
            {
                result.Successful = false;
                result.Information = reply.ValidationResponse.Information;
                return result;
            }

            foreach (var resultTeam in reply.Teams)
            {
                _teams.Add(new Teams
                {
                    ID = resultTeam.Id,
                    TeamName = resultTeam.TeamName,
                    Description = resultTeam.Descirption,
                    Status = (TeamStatusEnum)resultTeam.Status,
                    TeamJoinCode = resultTeam.TeamJoinCode,
                    TeamJoinPassword = resultTeam.TeamJoinPassword,
                    CreatedById = resultTeam.CreatedById,
                    CreatedOn = DateTime.Parse(resultTeam.CreatedOn),
                    UpdatedById = resultTeam.UpdatedById,
                    UpdatedOn = DateTime.Parse(resultTeam.UpdatedOn)
                });
            }

            result.ResultValues = _teams;
            result.Successful = true;
            return result;
        }

        public async Task<ValidationResponse> CreateNewTeamAsync(Teams team)
        {
            var reply = await ProcurementClient.CreateNewTeamAsync(new GRPCCreateNewTeam
            {
                Team = new GRPCTeam()
                {
                    TeamJoinPassword = team.TeamJoinPassword,
                    TeamName = team.TeamName,
                    Descirption = team.Description,
                },
                User = new GRPCLoginInformationForUser()
                {
                    Id = App.LoggedUserInApplication.Id.ToString(),
                    Password = App.LoggedUserInApplication.PasswordHash,
                    Username = App.LoggedUserInApplication.UserName,
                }
            });

            var result = new ValidationResponse()
            {
                Successful = reply.Successful,
                Information = reply.Information,
            };

            return result;
        }
    }
}
