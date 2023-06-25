using System.Diagnostics;
using System.Net;
using Grpc.Core;
using GrpcShared;
using GrpcShared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProcurementService.Data;
using ProcurementService.Data.Query;
using ProcurementService.Functions;

namespace ProcurementService.Services
{
    public class ProcurementService : Procurement.ProcurementBase
    {
        private readonly ILogger<ProcurementService> _logger;
        private readonly ProcurementContext _context;
        private readonly VerifyLogin _verifyLogin;
        private readonly CodeGenerator _codeGenerator;
        private readonly GetUsersQuery _usersQuery;

        public ProcurementService(ILogger<ProcurementService> logger, ProcurementContext context,
            VerifyLogin verifyLogin, CodeGenerator codeGenerator, GetUsersQuery usersQuery)
        {
            _logger = logger;
            _context = context;
            _verifyLogin = verifyLogin;
            _codeGenerator = codeGenerator;
            _usersQuery = usersQuery;
        }

        public override Task<GRPCValidationResponse> RegisterUser(GRPCRegisterNewUser request,
            ServerCallContext context)
        {
            _logger.LogInformation("Start of call Register User: " + DateTime.Now);
            if (request.Password != request.ConfirmPassword)
            {
                _logger.LogInformation("End of call Register User: " + DateTime.Now);

                return Task.FromResult(new GRPCValidationResponse()
                {
                    Successful = false,
                    Information = "Password and confirm password must be this same!"
                });
            }

            var passwordStrenghtScore = PasswordValidator.CheckStrength(request.Password);

            switch (passwordStrenghtScore)
            {
                case PasswordScore.Blank:
                case PasswordScore.VeryWeak:
                case PasswordScore.Weak:
                    _logger.LogInformation("End of call Register User: " + DateTime.Now);
                    return Task.FromResult(new GRPCValidationResponse()
                    {
                        Successful = false,
                        Information = "Given password is too week!"
                    });
            }

            var isEmailInUse = _context.Users.Any(e => e.UserName == request.Person.Email);
            if (isEmailInUse)
            {
                _logger.LogInformation("End of call Register User: " + DateTime.Now);
                return Task.FromResult(new GRPCValidationResponse()
                {
                    Successful = false,
                    Information = "Given email is already in use!"
                });
            }

            try
            {
                var person = new Persons
                {
                    FirstName = request.Person.FirstName,
                    LastName = request.Person.LastName,
                    Email = request.Person.Email,
                };

                _context.Add(person);
                _context.SaveChanges();

                var user = new Users
                {
                    Id = default,
                    UserName = request.Person.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    SecurityStamp = new Guid().ToString(),
                    PersonID = person.Id,
                    Disabled = false,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                };

                _context.Add(user);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Error in function RegisterUser: " + ex);

                return Task.FromResult(new GRPCValidationResponse()
                {
                    Successful = false,
                    Information = "There was a problem with connection! Please try again later."
                });
            }

            _logger.LogInformation("End of call Register User: " + DateTime.Now);
            return Task.FromResult(new GRPCValidationResponse()
            {
                Successful = true,
                Information = "Account was created successfully"
            });
        }

        public override Task<GRPCLoggedUser> LoginUserToApplication(GRPCLoginUser request, ServerCallContext context)
        {
            _logger.LogInformation("Start of call Login User to application: " + DateTime.Now);

            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                _logger.LogInformation("End of call Login User to application: " + DateTime.Now);
                return Task.FromResult(new GRPCLoggedUser()
                {
                    ValidationResponse = new GRPCValidationResponse()
                    {
                        Successful = false,
                        Information = "Entered values are empty!"
                    }
                });
            }

            var userVerify = _verifyLogin.CheckIfUsersExists(request.Email, request.Password);

            if (!userVerify.Successful)
            {
                _logger.LogInformation("End of call Login User to application: " + DateTime.Now);

                return Task.FromResult(new GRPCLoggedUser()
                {
                    ValidationResponse = new GRPCValidationResponse()
                    {
                        Successful = false,
                        Information = "There was a problem when verifying your login credentials!"
                    }
                });
            }

            _logger.LogInformation("End of call Login User to application: " + DateTime.Now);

            return _usersQuery.GetUserDataFromDb(new GRPCLoginInformationForUser()
                { Id = userVerify.Information, Password = request.Password, Username = request.Email });
        }

        public override Task<GRPCLoggedUser> GetUserData(GRPCLoginInformationForUser request, ServerCallContext context)
        {
            _logger.LogInformation("Start of call get user data: " + DateTime.Now);

            var isUsersExists =
                _verifyLogin.CheckIfUsersExists(request.Username, request.Password, Guid.Parse(request.Id));

            if (!isUsersExists.Successful)
            {
                _logger.LogInformation("End of call get user data: " + DateTime.Now);

                return Task.FromResult(new GRPCLoggedUser()
                {
                    ValidationResponse = new GRPCValidationResponse()
                    {
                        Successful = false,
                        Information = "There was a problem when verifying your login credentials!"
                    }
                });
            }

            _logger.LogInformation("End of call get user data: " + DateTime.Now);
            return _usersQuery.GetUserDataFromDb(request);
        }


        public override Task<GRPCTeamsList> GetTeamsList(GRPCLoginInformationForUser request, ServerCallContext context)
        {
            _logger.LogInformation("Start of call GetTeamsList: " + DateTime.Now);

            var isUsersExists =
                _verifyLogin.CheckIfUsersExists(request.Username, request.Password, Guid.Parse(request.Id));

            if (!isUsersExists.Successful)
            {
                _logger.LogInformation("End of call GetTeamsList: " + DateTime.Now);

                return Task.FromResult(new GRPCTeamsList
                {
                    ValidationResponse = new GRPCValidationResponse()
                    {
                        Successful = false,
                        Information = "There was a problem when verifying your login credentials!"
                    }
                });
            }

            try
            {
                var teams = _context.TeamMembers.Include(e => e.Team)
                    .Where(e => e.PersonID ==
                                (_context.Users.FirstOrDefault(_ => _.Id == Guid.Parse(request.Id))).PersonID);

                if (!teams.Any())
                {
                    _logger.LogInformation("End of call GetTeamsList: " + DateTime.Now);

                    return Task.FromResult(new GRPCTeamsList
                    {
                        ValidationResponse = new GRPCValidationResponse()
                        {
                            Successful = true,
                            Information = "No team was found"
                        }
                    });
                }

                var result = new List<GRPCTeam>();
                foreach (var team in teams)
                {
                    result.Add(new GRPCTeam
                    {
                        Id = team.Team.ID,
                        TeamName = team.Team.TeamName,
                        Descirption = team.Team.Description,
                        Status = (int)team.Team.Status,
                        TeamJoinCode = team.Team.TeamJoinCode,
                        TeamJoinPassword = team.Team.TeamJoinPassword,
                        CreatedById = team.Team.CreatedById,
                        CreatedOn = team.Team.CreatedOn.ToString(),
                        UpdatedById = team.Team.UpdatedById,
                        UpdatedOn = team.Team.UpdatedOn.ToString(),
                    });
                }

                _logger.LogInformation("End of call GetTeamsList: " + DateTime.Now);

                return Task.FromResult(new GRPCTeamsList()
                {
                    Teams = { result },
                    ValidationResponse = new GRPCValidationResponse
                    {
                        Successful = true,
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Error in function GetTeamsList: " + ex);

                return Task.FromResult(new GRPCTeamsList
                {
                    ValidationResponse = new GRPCValidationResponse()
                    {
                        Successful = false,
                        Information = "There was an error while getting teams list data! Please try again!",
                    }
                });
            }
        }

        public override Task<GRPCValidationResponse> CreateNewTeam(GRPCCreateNewTeam request, ServerCallContext context)
        {
            _logger.LogInformation("Start of call CreateNewTeam: " + DateTime.Now);

            var isUsersExists =
                _verifyLogin.CheckIfUsersExists(request.User.Username, request.User.Password,
                    Guid.Parse(request.User.Id));

            if (!isUsersExists.Successful)
            {
                _logger.LogInformation("End of call CreateNewTeam: " + DateTime.Now);

                return Task.FromResult(new GRPCValidationResponse()
                    {
                        Successful = false,
                        Information = "There was a problem when verifying your login credentials!",
                    }
                );
            }

            var user = _context.Users.First(e => e.Id == Guid.Parse(request.User.Id));
            
            try
            {
                var passwordStrenghtScore = PasswordValidator.CheckStrength(request.Team.TeamJoinPassword);

                switch (passwordStrenghtScore)
                {
                    case PasswordScore.Blank:
                    case PasswordScore.VeryWeak:
                    case PasswordScore.Weak:
                        _logger.LogInformation("End of call Register User: " + DateTime.Now);
                        return Task.FromResult(new GRPCValidationResponse()
                        {
                            Successful = false,
                            Information = "Given password is too week!"
                        });
                }

                var team = new Teams
                {
                    TeamName = request.Team.TeamName,
                    Description = request.Team.Descirption,
                    TeamJoinPassword = BCrypt.Net.BCrypt.HashPassword(request.Team.TeamJoinPassword), // TODO: Password checking for more security
                    TeamJoinCode = _codeGenerator.GenerateRandomCode(6, true),
                    CreatedOn = DateTime.Now,
                    CreatedById = user.PersonID.Value,
                    UpdatedOn = DateTime.Now,
                    UpdatedById = user.PersonID.Value
                };

                _context.Teams.Add(team);
                _context.SaveChanges();

                var newMember = new TeamMembers
                {
                    TeamID = team.ID,
                    PersonID = user.PersonID.Value,
                    Role = TeamRoleEnum.TeamAdministrator,
                };

                _context.TeamMembers.Add(newMember);
                _context.SaveChanges();

                _logger.LogInformation("End of call CreateNewTeam: " + DateTime.Now);

                return Task.FromResult(new GRPCValidationResponse
                {
                    Successful = true,
                    Information = "New team was created successfully! Join code to your team: " + team.TeamJoinCode,
                });
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Error in function CreateNewTeam: " + ex);
                return Task.FromResult(new GRPCValidationResponse
                {
                    Successful = false,
                    Information = "There was an error while creating a new team! Please try again!",
                });
            }
        }

        public override Task<GRPCTeamValidationResponse> JoinToTeam(GRPCJoinToTeam request, ServerCallContext context)
        {
            _logger.LogInformation("Start of call JoinToTeam: " + DateTime.Now);

            var isUsersExists =
                _verifyLogin.CheckIfUsersExists(request.User.Username, request.User.Password,
                    Guid.Parse(request.User.Id));

            if (!isUsersExists.Successful)
            {
                _logger.LogInformation("End of call JoinToTeam: " + DateTime.Now);

                return Task.FromResult(new GRPCTeamValidationResponse()
                {
                    Response = new GRPCValidationResponse()
                    {
                        Successful = false,
                        Information = "There was a problem when verifying your login credentials!",
                    },
                });
            }

            var user = _context.Users.First(e => e.Id == Guid.Parse(request.User.Id));

            try
            {
                var team = _context.Teams.Where(e => e.TeamJoinCode == request.TeamJoinCode).ToList()
                    .FirstOrDefault(e => BCrypt.Net.BCrypt.Verify(request.TeamJoinPassword, e.TeamJoinPassword));

                if (team == null)
                {
                    return Task.FromResult(new GRPCTeamValidationResponse()
                    {
                        Response = new GRPCValidationResponse()
                        {
                            Successful = false,
                            Information = "Given code and password are wrong! Didn't find any team with given credentials!",
                        },
                    });
                }

                var isAlreadyMember =
                    _context.TeamMembers.Any(e => e.TeamID == team.ID && e.PersonID == user.PersonID);

                if (isAlreadyMember)
                {
                    return Task.FromResult(new GRPCTeamValidationResponse()
                    {
                        Response = new GRPCValidationResponse()
                        {
                            Successful = false,
                            Information = "You are already member of this team!",
                        },
                    });
                }

                var newTeamMember = new TeamMembers()
                {
                    TeamID = team.ID,
                    PersonID = user.PersonID.Value,
                    Role = TeamRoleEnum.TeamMember,
                };

                _context.TeamMembers.Add(newTeamMember);
                _context.SaveChanges();

                return Task.FromResult(new GRPCTeamValidationResponse()
                {
                    Response = new GRPCValidationResponse()
                    {
                        Successful = true,
                        Information = "Joined to new team!",
                    },
                    Team = new GRPCTeam()
                    {
                        Id = team.ID,
                        TeamName = team.TeamName,
                        Descirption = team.Description,
                        Status = (int)team.Status,
                        TeamJoinCode = team.TeamJoinCode,
                        TeamJoinPassword = team.TeamJoinPassword,
                        CreatedById = team.CreatedById,
                        CreatedOn = team.CreatedOn.ToString(),
                        UpdatedById = team.UpdatedById,
                        UpdatedOn = team.UpdatedOn.ToString(),
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Error in function JoinToTeam: " + ex);

                return Task.FromResult(new GRPCTeamValidationResponse()
                {
                    Response = new GRPCValidationResponse()
                    {
                        Successful = false,
                        Information = "There was an error while joining to team! Please try again!",
                    },
                });
            }
        }

        public override Task<GRPCSelectedTeamResponse> GetSelectedTeam(GRPCGetSelectedTeamRequest request, ServerCallContext context)
        {
	        _logger.LogInformation("Start of call GetSelectedTeam: " + DateTime.Now);

	        var userExistsValidationResult =
		        _verifyLogin.CheckIfUsersExists(request.LoggedUser.Username, request.LoggedUser.Password, Guid.Parse(request.LoggedUser.Id));

	        if (!userExistsValidationResult.Successful)
	        {
		        _logger.LogInformation("End of call GetSelectedTeam: " + DateTime.Now);

		        return Task.FromResult(new GRPCSelectedTeamResponse
				{
			        Response = new GRPCValidationResponse()
			        {
				        Successful = false,
				        Information = "There was a problem when verifying your login credentials!"
			        }
		        });
	        }

	        var personIdOfLoggedUser = int.Parse(userExistsValidationResult.Information);

			try
	        {
                var team = _context.Teams.FirstOrDefault(e => e.ID == request.TeamId && e.Status != TeamStatusEnum.Removed);
		        if (team == null)
		        {
			        return Task.FromResult(new GRPCSelectedTeamResponse()
			        {
				        Response = new GRPCValidationResponse()
				        {
					        Successful = false,
					        Information = "Couldn't find selected team or it has been removed!",
				        },
			        });
				}

		        var member = _context.TeamMembers.FirstOrDefault(e => e.PersonID == personIdOfLoggedUser && e.TeamID == request.TeamId);

		        if (member == null)
		        {
					return Task.FromResult(new GRPCSelectedTeamResponse()
					{
						Response = new GRPCValidationResponse()
						{
							Successful = false,
							Information = "User not found! Try again",
						},
					});
		        }
				return Task.FromResult(new GRPCSelectedTeamResponse
			        {
				        Id = team.ID,
				        TeamName = team.TeamName,
				        Descirption = team.Description,
				        Status = (int)team.Status,
				        Role = (int)member.Role,
                        Response = new GRPCValidationResponse()
                        {
	                        Successful = true,
	                        Information = "",
                        }
				});
	        }
	        catch (Exception ex)
	        {
				_logger.LogCritical($"Error in function GetSelectedTeam: Date of error: {DateTime.Now} Error: {ex}");

				return Task.FromResult(new GRPCSelectedTeamResponse()
				{
					Response = new GRPCValidationResponse()
					{
						Successful = false,
						Information = "There was an error while getting selected team! Please try again!",
					},
				});
			}
        }
    }
}