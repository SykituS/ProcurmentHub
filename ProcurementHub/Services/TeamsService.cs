using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrpcShared;
using ProcurementHub.Model;
using ProcurementHub.Model.CustomModels;
using ProcurementHub.Model.Models;

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
                    Descirption = team.Description ?? "",
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

        public async Task<ValidationResponseWithResult<Teams>> JoinToTeam(Teams team)
        {
            var reply = await ProcurementClient.JoinToTeamAsync(new GRPCJoinToTeam()
            {
                TeamJoinPassword = team.TeamJoinPassword,
                TeamJoinCode = team.TeamJoinCode,
                User = new GRPCLoginInformationForUser()
                {
                    Id = App.LoggedUserInApplication.Id.ToString(),
                    Password = App.LoggedUserInApplication.PasswordHash,
                    Username = App.LoggedUserInApplication.UserName,
                }
            });

            var result = new ValidationResponseWithResult<Teams>
            {
                Successful = reply.Response.Successful,
                Information = reply.Response.Information,
            };

            if (result.Successful)
            {
                result.ResultValues = new Teams
                {
                    ID = reply.Team.Id,
                    TeamName = reply.Team.TeamName,
                    Description = reply.Team.Descirption,
                    Status = (TeamStatusEnum)reply.Team.Status,
                    TeamJoinCode = reply.Team.TeamJoinCode,
                    TeamJoinPassword = reply.Team.TeamJoinPassword,
                    CreatedById = reply.Team.CreatedById,
                    CreatedOn = DateTime.Parse(reply.Team.CreatedOn),
                    UpdatedById = reply.Team.UpdatedById,
                    UpdatedOn = DateTime.Parse(reply.Team.UpdatedOn),
                };
            }

            return result;
        }

        public async Task<ValidationResponseWithResult<TeamMainModel>> GetSelectedTeam(int teamId)
        {
	        var reply = await ProcurementClient.GetSelectedTeamAsync(new GRPCGetInformationForGivenTeamRequest
			{
		        LoggedUser = new GRPCLoginInformationForUser()
		        {
			        Id = App.LoggedUserInApplication.Id.ToString(),
			        Password = App.LoggedUserInApplication.PasswordHash,
			        Username = App.LoggedUserInApplication.UserName,
		        },
		        TeamId = teamId,
	        });

            var result = new ValidationResponseWithResult<TeamMainModel>
            {
	            Successful = reply.Response.Successful,
	            Information = reply.Response.Information,
            };

            if (result.Successful)
            {
	            result.ResultValues = new TeamMainModel
	            {
		            ID = reply.Id,
		            TeamName = reply.TeamName,
		            Description = reply.Descirption,
		            Status = (TeamStatusEnum)reply.Status,
		            Role = (TeamRoleEnum)reply.Role,
	            };
            }

            return result;
        }
    }
}
