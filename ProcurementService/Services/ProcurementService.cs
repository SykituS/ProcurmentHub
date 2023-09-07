using System.Diagnostics;
using System.Net;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcShared;
using GrpcShared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProcurementService.Data;
using ProcurementService.Data.Query;
using ProcurementService.DbModels;
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

        public override Task<GRPCSelectedTeamResponse> GetSelectedTeam(GRPCGetInformationForGivenIdRequest request, ServerCallContext context)
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
                var team = _context.Teams.FirstOrDefault(e => e.ID == request.Id && e.Status != TeamStatusEnum.Removed);
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

                var member = _context.TeamMembers.FirstOrDefault(e => e.PersonID == personIdOfLoggedUser && e.TeamID == request.Id);

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

        public override Task<GRPCValidationResponse> CreateOrUpdateRestaurant(GRPCCreateOrUpdateRestaurantRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Start of call CreateOrUpdateRestaurant: " + DateTime.Now);
            var userExistsValidationResult =
                _verifyLogin.CheckIfUsersExists(request.LoggedUser.Username, request.LoggedUser.Password, Guid.Parse(request.LoggedUser.Id));

            if (!userExistsValidationResult.Successful)
            {
                _logger.LogInformation("End of call CreateOrUpdateRestaurant: " + DateTime.Now);

                return Task.FromResult(new GRPCValidationResponse()
                {
                    Successful = false,
                    Information = "There was a problem when verifying your login credentials!"

                });
            }

            var personIdOfLoggedUser = int.Parse(userExistsValidationResult.Information);

            try
            {
                var restaurant = new TeamRestaurants();
                if (request.Id != 0)
                    restaurant = _context.TeamRestaurants.Find(request.Id);

                if (restaurant != null)
                {
                    restaurant.Address = request.Address;
                    restaurant.Description = request.Description;
                    restaurant.Name = request.Name;
                    restaurant.TeamID = request.TeamId;

                    var datetimeNow = DateTime.UtcNow;

                    restaurant.UpdatedById = personIdOfLoggedUser;
                    restaurant.UpdatedOn = datetimeNow;
                    if (request.Id != 0)
                    {
                        restaurant.IsDeleted = request.IsDeleted;
                        _context.Entry(restaurant).State = EntityState.Modified;
                    }
                    else
                    {
                        restaurant.IsDeleted = false;
                        restaurant.CreatedById = personIdOfLoggedUser;
                        restaurant.CreatedOn = datetimeNow;
                        _context.Entry(restaurant).State = EntityState.Added;
                    }
                }
                else
                {
                    _logger.LogInformation("End of call CreateOrUpdateRestaurant: " + DateTime.Now);

                    return Task.FromResult(new GRPCValidationResponse()
                    {
                        Successful = false,
                        Information = "There was a problem when updating restaurant"

                    });
                }

                _context.SaveChanges();


                _logger.LogInformation("End of call CreateOrUpdateRestaurant: " + DateTime.Now);

                return Task.FromResult(new GRPCValidationResponse()
                {
                    Successful = true,
                    Information = "Restaurant was created successfully!"

                });
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Error in function CreateOrUpdateRestaurant: Date of error: {DateTime.Now} Error: {ex}");

                return Task.FromResult(new GRPCValidationResponse()
                {
                    Successful = false,
                    Information = "There was an error while creating new restaurant! Please try again!",

                });

            }
        }

        public override Task<GRPCTeamRestaurnatsListResponse> GetTeamRestaurantList(GRPCGetInformationForGivenIdRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Start of call GetTeamRestaurantList: " + DateTime.Now);
            var userExistsValidationResult =
                _verifyLogin.CheckIfUsersExists(request.LoggedUser.Username, request.LoggedUser.Password, Guid.Parse(request.LoggedUser.Id));

            if (!userExistsValidationResult.Successful)
            {
                _logger.LogInformation("End of call GetTeamRestaurantList: " + DateTime.Now);

                return Task.FromResult(new GRPCTeamRestaurnatsListResponse
                {
                    Response = new GRPCValidationResponse()
                    {
                        Successful = false,
                        Information = "There was a problem when verifying your login credentials!"
                    }
                });
            }

            try
            {
                var restaurants = _context.TeamRestaurants
                    .Where(e => e.TeamID == request.Id && !e.IsDeleted)
                    .Include(e => e.CreatedBy)
                    .Include(e => e.UpdatedBy)
                    .ToList();

                var result = new List<GRPCRestaurant>();
                foreach (var item in restaurants)
                {
                    result.Add(new GRPCRestaurant
                    {
                        Id = item.ID,
                        Name = item.Name,
                        Address = item.Address,
                        Description = item.Description,
                        CreatedBy = new GRPCPerson
                        {
                            Id = item.CreatedBy.Id,
                            FirstName = item.CreatedBy.FirstName,
                            LastName = item.CreatedBy.LastName,
                            Email = item.CreatedBy.Email,
                        },
                        CreatedOn = item.CreatedOn.ToUniversalTime().ToTimestamp(),
                        UpdatedBy = new GRPCPerson()
                        {
                            Id = item.UpdatedBy.Id,
                            FirstName = item.UpdatedBy.FirstName,
                            LastName = item.UpdatedBy.LastName,
                            Email = item.UpdatedBy.Email,
                        },
                        UpdatedOn = item.UpdatedOn.ToUniversalTime().ToTimestamp()
                    });
                }

                _logger.LogInformation("End of call GetTeamRestaurantList: " + DateTime.Now);

                return Task.FromResult(new GRPCTeamRestaurnatsListResponse()
                {
                    RestaurantList = { result },
                    Response = new GRPCValidationResponse
                    {
                        Successful = true,
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Error in function GetTeamRestaurantList: Date of error: {DateTime.Now} Error: {ex}");

                return Task.FromResult(new GRPCTeamRestaurnatsListResponse
                {
                    Response = new GRPCValidationResponse()
                    {
                        Successful = false,
                        Information = "There was an error while getting list of restaurants! Please try again!",
                    }
                });
            }
        }

        public override Task<GRPCValidationResponse> CreateOrUpdateRestaurantItem(GRPCCreateOrUpdateRestaurantItemRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Start of call CreateOrUpdateRestaurant: " + DateTime.Now);
            var userExistsValidationResult =
                _verifyLogin.CheckIfUsersExists(request.LoggedUser.Username, request.LoggedUser.Password, Guid.Parse(request.LoggedUser.Id));

            if (!userExistsValidationResult.Successful)
            {
                _logger.LogInformation("End of call CreateOrUpdateRestaurant: " + DateTime.Now);

                return Task.FromResult(new GRPCValidationResponse()
                {
                    Successful = false,
                    Information = "There was a problem when verifying your login credentials!"

                });
            }

            var personIdOfLoggedUser = int.Parse(userExistsValidationResult.Information);

            try
            {
                var restaurantItem = new TeamRestaurantsItems();
                if (request.Id != 0)
                    restaurantItem = _context.TeamRestaurantsItems.Find(request.Id);

                if (restaurantItem != null)
                {
                    restaurantItem.Description = request.Description;
                    restaurantItem.Name = request.Name;
                    restaurantItem.Price = decimal.Parse(request.Price.Price);
                    restaurantItem.CurrencyType = request.Price.CurrencyCode;
                    restaurantItem.TeamRestaurantID = request.RestaurantId;

                    var datetimeNow = DateTime.UtcNow;

                    restaurantItem.UpdatedById = personIdOfLoggedUser;
                    restaurantItem.UpdatedOn = datetimeNow;
                    if (request.Id != 0)
                    {
                        restaurantItem.IsDeleted = request.IsDeleted;
                        _context.Entry(restaurantItem).State = EntityState.Modified;
                    }
                    else
                    {
                        restaurantItem.IsDeleted = false;
                        restaurantItem.CreatedById = personIdOfLoggedUser;
                        restaurantItem.CreatedOn = datetimeNow;
                        _context.Entry(restaurantItem).State = EntityState.Added;
                    }
                }
                else
                {
                    _logger.LogInformation("End of call CreateOrUpdateRestaurant: " + DateTime.Now);

                    return Task.FromResult(new GRPCValidationResponse()
                    {
                        Successful = false,
                        Information = "There was a problem when updating restaurant"

                    });
                }

                _context.SaveChanges();


                _logger.LogInformation("End of call CreateOrUpdateRestaurant: " + DateTime.Now);

                return Task.FromResult(new GRPCValidationResponse()
                {
                    Successful = true,
                    Information = "Restaurant was created successfully!"

                });
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Error in function CreateOrUpdateRestaurant: Date of error: {DateTime.Now} Error: {ex}");

                return Task.FromResult(new GRPCValidationResponse()
                {
                    Successful = false,
                    Information = "There was an error while creating new restaurant! Please try again!",

                });

            }
        }

        public override Task<GRPCTeamRestaurnatItemsListResponse> GetTeamRestaurantItemList(GRPCGetInformationForGivenIdRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Start of call GetTeamRestaurantItemList: " + DateTime.Now);
            var userExistsValidationResult =
                _verifyLogin.CheckIfUsersExists(request.LoggedUser.Username, request.LoggedUser.Password, Guid.Parse(request.LoggedUser.Id));

            if (!userExistsValidationResult.Successful)
            {
                _logger.LogInformation("End of call GetTeamRestaurantItemList: " + DateTime.Now);

                return Task.FromResult(new GRPCTeamRestaurnatItemsListResponse()
                {
                    Response = new GRPCValidationResponse()
                    {
                        Successful = false,
                        Information = "There was a problem when verifying your login credentials!"
                    }
                });
            }

            try
            {
                var restaurantItems = _context.TeamRestaurantsItems
                    .Where(e => e.TeamRestaurantID == request.Id && !e.IsDeleted)
                    .Include(e => e.CreatedBy)
                    .Include(e => e.UpdatedBy)
                    .ToList();

                var result = new List<GRPCRestaurantItem>();
                foreach (var item in restaurantItems)
                {
                    result.Add(new GRPCRestaurantItem()
                    {
                        Id = item.ID,
                        Name = item.Name,
                        Price = new money
                        {
                            CurrencyCode = item.CurrencyType,
                            Price = item.Price.ToString(),
                        },
                        Description = item.Description,
                        CreatedBy = new GRPCPerson
                        {
                            Id = item.CreatedBy.Id,
                            FirstName = item.CreatedBy.FirstName,
                            LastName = item.CreatedBy.LastName,
                            Email = item.CreatedBy.Email,
                        },
                        CreatedOn = item.CreatedOn.ToUniversalTime().ToTimestamp(),
                        UpdatedBy = new GRPCPerson()
                        {
                            Id = item.UpdatedBy.Id,
                            FirstName = item.UpdatedBy.FirstName,
                            LastName = item.UpdatedBy.LastName,
                            Email = item.UpdatedBy.Email,
                        },
                        UpdatedOn = item.UpdatedOn.ToUniversalTime().ToTimestamp()
                    });
                }

                _logger.LogInformation("End of call GetTeamRestaurantItemList: " + DateTime.Now);

                return Task.FromResult(new GRPCTeamRestaurnatItemsListResponse()
                {
                    RestaurantItemList = { result },
                    Response = new GRPCValidationResponse
                    {
                        Successful = true,
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Error in function GetTeamRestaurantItemList: Date of error: {DateTime.Now} Error: {ex}");

                return Task.FromResult(new GRPCTeamRestaurnatItemsListResponse
                {
                    Response = new GRPCValidationResponse()
                    {
                        Successful = false,
                        Information = "There was an error while getting list of restaurants! Please try again!",
                    }
                });
            }
        }

        public override Task<GRPCOrderInformationsResponse> StartNewOrder(GRPCStartNewOrderRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Start of call StartNewOrder: " + DateTime.Now);
            var userExistsValidationResult =
                _verifyLogin.CheckIfUsersExists(request.LoggedUser.Username, request.LoggedUser.Password, Guid.Parse(request.LoggedUser.Id));

            if (!userExistsValidationResult.Successful)
            {
                _logger.LogInformation("End of call StartNewOrder: " + DateTime.Now);

                return Task.FromResult(new GRPCOrderInformationsResponse()
                {
                    Response = new GRPCValidationResponse()
                    {
                        Successful = false,
                        Information = "There was a problem when verifying your login credentials!"
                    }
                });
            }

            var personIdOfLoggedUser = int.Parse(userExistsValidationResult.Information);
            var datetimeNow = DateTime.UtcNow;

            try
            {
                var order = new TeamOrders
                {
                    ID = Guid.NewGuid(),
                    TeamID = request.TeamId,
                    TeamRestaurantID = request.RestaurantId,
                    Status = TeamOrderStatusEnum.InProgress,
                    OrderStartedByID = personIdOfLoggedUser,
                    OrderStartedOn = datetimeNow,
                    OrderPayedByID = personIdOfLoggedUser,
                };

                _context.Entry(order).State = EntityState.Added;
                _context.SaveChanges();

                return Task.FromResult(new GRPCOrderInformationsResponse()
                {
                    Order = new GRPCOrderInformations
                    {
                        Id = order.ID.ToString(),
                        TeamId = order.TeamID,
                        RestaurantId = order.TeamRestaurantID,
                        Status = (int)order.Status,
                        OrderStartedBy = order.OrderStartedByID,
                        StartedOn = order.OrderStartedOn.ToTimestamp(),
                        OrderPayedById = order.OrderPayedByID.Value,
                    },
                    Response = new GRPCValidationResponse()
                    {
                        Successful = true,
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Error in function StartNewOrder: Date of error: {DateTime.Now} Error: {ex}");

                return Task.FromResult(new GRPCOrderInformationsResponse()
                {
                    Response = new GRPCValidationResponse()
                    {
                        Successful = false,
                        Information = "There was an error while starting new order! Please try again!"
                    }
                });
            }
        }

        public override Task<GRPCValidationResponse> AddItemsToOrder(GRPCOrderAddItems request, ServerCallContext context)
        {
            _logger.LogInformation("Start of call AddItemsToOrder: " + DateTime.Now);
            var userExistsValidationResult =
                _verifyLogin.CheckIfUsersExists(request.LoggedUser.Username, request.LoggedUser.Password, Guid.Parse(request.LoggedUser.Id));

            if (!userExistsValidationResult.Successful)
            {
                _logger.LogInformation("End of call AddItemsToOrder: " + DateTime.Now);

                return Task.FromResult(new GRPCValidationResponse()
                {
                    Successful = false,
                    Information = "There was a problem when verifying your login credentials!"
                });
            }

            var personIdOfLoggedUser = int.Parse(userExistsValidationResult.Information);
            var datetimeNow = DateTime.UtcNow;

            try
            {
                var listOfItems = request.Items.Select(item => new TeamOrdersItems
                {
                    TeamOrdersID = Guid.Parse(request.OrderId),
                    TeamRestaurantsItemsID = item.TeamRestaurantsItemId,
                    Quantity = item.Quantity,
                    TotalPriceOfItem = decimal.Parse(item.TotalPriceOfItem.Price),
                    ItemSelectedByID = personIdOfLoggedUser,
                    DivideToken = item.DivideToken.Any() ? Guid.Parse(item.DivideToken) : null,
                    DivideOnNumberOfPersons = item.DivideOnNumberOfPersons,
                    DividedPrice = item.DividePrice.Price.Any() ? decimal.Parse(item.DividePrice.Price) : null,
                })
                    .ToList();

                _context.TeamOrdersItems.AddRange(listOfItems);
                _context.SaveChanges();

                return Task.FromResult(new GRPCValidationResponse()
                {
                    Successful = true,
                    Information = ""
                });
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Error in function AddItemsToOrder: Date of error: {DateTime.Now} Error: {ex}");

                return Task.FromResult(new GRPCValidationResponse()
                {
                    Successful = false,
                    Information = "There was an error while starting new order! Please try again!"
                });
            }
        }

        public override Task<GRPCFullOrderDetailsResponse> GetFullOrderDetailsById(GRPCGetOrderByIdRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Start of call GetFullOrderDetailsById: " + DateTime.Now);
            var userExistsValidationResult =
                _verifyLogin.CheckIfUsersExists(request.LoggedUser.Username, request.LoggedUser.Password, Guid.Parse(request.LoggedUser.Id));

            if (!userExistsValidationResult.Successful)
            {
                _logger.LogInformation("End of call GetFullOrderDetailsById: " + DateTime.Now);

                return Task.FromResult(new GRPCFullOrderDetailsResponse()
                {
                    Response = new GRPCValidationResponse()
                    {
                        Successful = false,
                        Information = "There was a problem when verifying your login credentials!"
                    }
                });
            }

            try
            {
                var order = _context.TeamOrders
                    .Include(e => e.Team)
                    .Include(e => e.OrderPayedBy)
                    .Include(e => e.OrderStartedBy)
                    .Include(e => e.TeamRestaurant)
                    .FirstOrDefault(e => e.ID == Guid.Parse(request.OrderId));
                if (order == null)
                {
                    return Task.FromResult(new GRPCFullOrderDetailsResponse()
                    {
                        Response = new GRPCValidationResponse()
                        {
                            Successful = false,
                            Information = "Order was not found!"
                        }
                    });
                }

                var orderItems = _context.TeamOrdersItems
                    .Include(e => e.TeamRestaurantsItems)
                    .Include(e => e.ItemSelectedBy)
                    .Where(e => e.TeamOrdersID == order.ID);

                var response = new GRPCFullOrderDetailsResponse()
                {
                    Order = new GRPCFullOrderInformations
                    {
                        Id = order.ID.ToString(),
                        TeamId = order.TeamID,
                        RestaurantId = order.TeamRestaurantID,
                        Status = (int)order.Status,
                        OrderStartedById = order.OrderStartedByID,
                        StartedOn = order.OrderStartedOn.ToUniversalTime().ToTimestamp(),
                        OrderPayedById = order.OrderPayedByID ?? 0,
                        FinishedOn = order.OrderFinishedOn?.ToUniversalTime().ToTimestamp(),
                        TotalPriceOfOrder = new money
                        {
                            CurrencyCode = "PLN",
                            Price = order.TotalPriceOfOrder.ToString() == "" ? decimal.Zero.ToString() : order.TotalPriceOfOrder.ToString(),
                        },
                        Restaurant = new GRPCRestaurant
                        {
                            Id = order.TeamRestaurant.ID,
                            Name = order.TeamRestaurant.Name,
                            Address = order.TeamRestaurant.Address,
                            Description = order.TeamRestaurant.Description,
                        },
                        OrderStartedBy = new GRPCPerson
                        {
                            FirstName = order.OrderStartedBy.FirstName,
                            LastName = order.OrderStartedBy.LastName,
                        },
                        OrderPayedBy = new GRPCPerson
                        {
                            FirstName = order.OrderPayedBy.FirstName,
                            LastName = order.OrderPayedBy.LastName,
                        },
                    },
                    Response = new GRPCValidationResponse(),
                };

                foreach (var orderItem in orderItems)
                {
                    response.Items.Add(new GRPCFullOrderItem
                    {
                        Id = orderItem.ID,
                        TeamOrderId = orderItem.TeamOrdersID.ToString(),
                        TeamRestaurantsItemId = orderItem.TeamRestaurantsItemsID,
                        Quantity = orderItem.Quantity,
                        TotalPriceOfItem = new money
                        {
                            CurrencyCode = "PLN",
                            Price = orderItem.TotalPriceOfItem.ToString() ?? decimal.Zero.ToString(),
                        },
                        DivideToken = orderItem.DivideToken?.ToString() ?? string.Empty,
                        DivideOnNumberOfPersons = orderItem.DivideOnNumberOfPersons ?? 0,
                        DividePrice = new money
                        {
                            CurrencyCode = "PLN",
                            Price = orderItem.DividedPrice?.ToString() ?? decimal.Zero.ToString(),
                        },
                        RestaurantItem = new GRPCRestaurantItem
                        {
                            Id = orderItem.TeamRestaurantsItems.ID,
                            Name = orderItem.TeamRestaurantsItems.Name,
                            Price = new money
                            {
                                CurrencyCode = orderItem.TeamRestaurantsItems.CurrencyType,
                                Price = orderItem.TeamRestaurantsItems.Price.ToString() == "" ? decimal.Zero.ToString() : orderItem.TeamRestaurantsItems.Price.ToString(),
                            },
                            Description = orderItem.TeamRestaurantsItems.Description,
                        },
                        SelectedById = orderItem.ItemSelectedByID,
                        SelectedBy = new GRPCPerson
                        {
                            FirstName = orderItem.ItemSelectedBy.FirstName,
                            LastName = orderItem.ItemSelectedBy.FirstName,
                        },
                    });
                }

                response.Response.Successful = true;

                return Task.FromResult(response);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Error in function GetFullOrderDetailsById: Date of error: {DateTime.Now} Error: {ex}");
                return Task.FromResult(new GRPCFullOrderDetailsResponse()
                {
                    Response = new GRPCValidationResponse()
                    {
                        Successful = false,
                        Information = "There was an error while starting new order! Please try again!"
                    }
                });
            }
        }

        public override Task<GRPCValidationResponse> CloseOrderById(GRPCGetOrderByIdRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Start of call CloseOrderById: " + DateTime.Now);
            var userExistsValidationResult =
                _verifyLogin.CheckIfUsersExists(request.LoggedUser.Username, request.LoggedUser.Password, Guid.Parse(request.LoggedUser.Id));

            if (!userExistsValidationResult.Successful)
            {
                _logger.LogInformation("End of call CloseOrderById: " + DateTime.Now);

                return Task.FromResult(new GRPCValidationResponse()
                {
                    Successful = false,
                    Information = "There was a problem when verifying your login credentials!"
                });
            }

            var personIdOfLoggedUser = int.Parse(userExistsValidationResult.Information);
            var dateTimeNow = DateTime.UtcNow;

            try
            {
                var order = _context.TeamOrders.FirstOrDefault(e => e.ID == Guid.Parse(request.OrderId));
                if (order == null)
                {
                    return Task.FromResult(new GRPCValidationResponse
                    {
                        Successful = false,
                        Information = "Order not found"
                    });
                }

                order.OrderFinishedOn = dateTimeNow;
                order.Status = TeamOrderStatusEnum.Closed;
                order.TotalPriceOfOrder = _context.TeamOrdersItems.Where(e => e.TeamOrdersID == order.ID).Select(e => e.TotalPriceOfItem).Sum();
                order.OrderPayedByID = personIdOfLoggedUser;

                _context.Entry(order).State = EntityState.Modified;
                _context.SaveChanges();

                return Task.FromResult(new GRPCValidationResponse()
                {
                    Successful = true
                });
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Error in function CloseOrderById: Date of error: {DateTime.Now} Error: {ex}");
                return Task.FromResult(new GRPCValidationResponse()
                {
                    Successful = false,
                    Information = "There was an error while starting new order! Please try again!"
                });
            }
        }

        public override Task<GRPCOrderListResponse> GetOrderListForTeamId(GRPCGetInformationForGivenIdRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Start of call GetOrderListForTeamId: " + DateTime.Now);
            var userExistsValidationResult =
                _verifyLogin.CheckIfUsersExists(request.LoggedUser.Username, request.LoggedUser.Password, Guid.Parse(request.LoggedUser.Id));

            if (!userExistsValidationResult.Successful)
            {
                _logger.LogInformation("End of call GetOrderListForTeamId: " + DateTime.Now);

                return Task.FromResult(new GRPCOrderListResponse
                {
                    Response = new GRPCValidationResponse()
                    {
                        Successful = false,
                        Information = "There was a problem when verifying your login credentials!"
                    }
                });
            }

            try
            {
                var orders = _context.TeamOrders
                    .Include(e => e.Team)
                    .Include(e => e.OrderPayedBy)
                    .Include(e => e.OrderStartedBy)
                    .Include(e => e.TeamRestaurant)
                    .Where(e => e.TeamID == request.Id);
                var response = new GRPCOrderListResponse()
                {
                    Response = new GRPCValidationResponse()
                };
                foreach (var order in orders)
                {
                    response.Orders.Add(new GRPCFullOrderInformations
                    {
                        Id = order.ID.ToString(),
                        TeamId = order.TeamID,
                        RestaurantId = order.TeamRestaurantID,
                        Status = (int)order.Status,
                        OrderStartedById = order.OrderStartedByID,
                        StartedOn = order.OrderStartedOn.ToUniversalTime().ToTimestamp(),
                        OrderPayedById = order.OrderPayedByID ?? 0,
                        FinishedOn = order.OrderFinishedOn?.ToUniversalTime().ToTimestamp(),
                        TotalPriceOfOrder = new money
                        {
                            CurrencyCode = "PLN",
                            Price = order.TotalPriceOfOrder.ToString() == "" ? decimal.Zero.ToString() : order.TotalPriceOfOrder.ToString(),
                        },
                        Restaurant = new GRPCRestaurant
                        {
                            Id = order.TeamRestaurant.ID,
                            Name = order.TeamRestaurant.Name,
                            Address = order.TeamRestaurant.Address,
                            Description = order.TeamRestaurant.Description,
                        },
                        OrderStartedBy = new GRPCPerson
                        {
                            FirstName = order.OrderStartedBy.FirstName,
                            LastName = order.OrderStartedBy.LastName,
                        },
                        OrderPayedBy = new GRPCPerson
                        {
                            FirstName = order.OrderPayedBy.FirstName,
                            LastName = order.OrderPayedBy.LastName,
                        },
                    });
                }

                response.Response.Successful = true;
                return Task.FromResult(response);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Error in function GetOrderListForTeamId: Date of error: {DateTime.Now} Error: {ex}");
                return Task.FromResult(new GRPCOrderListResponse
                {
                    Response = new GRPCValidationResponse()
                    {
                        Successful = false,
                        Information = "There was an error while starting new order! Please try again!"
                    }
                });
            }
        }

        public override Task<GRPCTeamMembersResponse> GetTeamMemebers(GRPCGetInformationForGivenIdRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Start of call GetTeamMemebers: " + DateTime.Now);
            var userExistsValidationResult =
                _verifyLogin.CheckIfUsersExists(request.LoggedUser.Username, request.LoggedUser.Password, Guid.Parse(request.LoggedUser.Id));

            if (!userExistsValidationResult.Successful)
            {
                _logger.LogInformation("End of call GetTeamMemebers: " + DateTime.Now);

                return Task.FromResult(new GRPCTeamMembersResponse()
                {
                    Response = new GRPCValidationResponse()
                    {
                        Successful = false,
                        Information = "There was a problem when verifying your login credentials!"
                    }
                });
            }

            try
            {
                var team = _context.Teams.FirstOrDefault(e => e.ID == request.Id);
                if (team == null)
                {
                    return Task.FromResult(new GRPCTeamMembersResponse
                    {
                        Response = new GRPCValidationResponse()
                        {
                            Successful = false,
                            Information = "Team was not found"
                        }
                    });
                }

                var teamMembers = _context.TeamMembers.Include(e => e.Person).Where(e => e.TeamID == team.ID);

                var reply = new GRPCTeamMembersResponse()
                {
                    Response = new GRPCValidationResponse()
                };

                var orderItems = _context.TeamOrdersItems
                    .Include(e => e.TeamOrders)
                    .Where(e => e.TeamOrders.TeamID == team.ID && e.TeamOrders.Status == TeamOrderStatusEnum.Closed).ToList();

                var orders = _context.TeamOrders.Where(e => e.TeamID == team.ID && e.Status == TeamOrderStatusEnum.Closed).ToList();

                foreach (var member in teamMembers)
                {
                    decimal? spendAmount = orderItems.Where(e => e.ItemSelectedByID == member.PersonID).Sum(e => e.TotalPriceOfItem);
                    decimal? payedAmount = orders.Where(e => e.OrderPayedByID == member.PersonID).Sum(e => e.TotalPriceOfOrder);

                    var spend = spendAmount ?? 0;
                    var payed = payedAmount ?? 0;
                    var ratio = payed - spend;

                    reply.TeamMembers.Add(new GRPCTeamMember
                    {
                        Id = member.ID,
                        FirstName = member.Person.FirstName,
                        LastName = member.Person.LastName,
                        SpendAmmonut = new money
                        {
                            CurrencyCode = "PLN",
                            Price = spendAmount.ToString() == "" ? decimal.Zero.ToString() : spendAmount.ToString()
                        },
                        PayedAmmonut = new money
                        {
                            CurrencyCode = "PLN",
                            Price = payedAmount.ToString() == "" ? decimal.Zero.ToString() : payedAmount.ToString()
                        },
                        PayedSpendRatio = new money
                        {
                            CurrencyCode = "PLN",
                            Price = ratio.ToString() == "" ? decimal.Zero.ToString() : ratio.ToString()
                        },
                        IsGroupCreator = member.Role == TeamRoleEnum.TeamAdministrator
                    });
                }

                reply.Response.Successful = true;
                return Task.FromResult(reply);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Error in function GetTeamMemebers: Date of error: {DateTime.Now} Error: {ex}");
                return Task.FromResult(new GRPCTeamMembersResponse
                {
                    Response = new GRPCValidationResponse()
                    {
                        Successful = false,
                        Information = "There was an error while starting new order! Please try again!"
                    }
                });
            }
        }
    }
}