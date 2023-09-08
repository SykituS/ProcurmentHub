using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcShared;
using GrpcShared.Models;
using Microsoft.EntityFrameworkCore;

namespace ProcurementService.Services
{
    public partial class ProcurementService
    {
        public override Task<GRPCValidationResponse> CreateOrUpdateRestaurant(
            GRPCCreateOrUpdateRestaurantRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Start of call CreateOrUpdateRestaurant: " + DateTime.Now);
            var userExistsValidationResult =
                _verifyLogin.CheckIfUsersExists(request.LoggedUser.Username, request.LoggedUser.Password,
                    Guid.Parse(request.LoggedUser.Id));

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
                _logger.LogCritical(
                    $"Error in function CreateOrUpdateRestaurant: Date of error: {DateTime.Now} Error: {ex}");

                return Task.FromResult(new GRPCValidationResponse()
                {
                    Successful = false,
                    Information = "There was an error while creating new restaurant! Please try again!",
                });
            }
        }

        public override Task<GRPCTeamRestaurnatsListResponse> GetTeamRestaurantList(
            GRPCGetInformationForGivenIdRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Start of call GetTeamRestaurantList: " + DateTime.Now);
            var userExistsValidationResult =
                _verifyLogin.CheckIfUsersExists(request.LoggedUser.Username, request.LoggedUser.Password,
                    Guid.Parse(request.LoggedUser.Id));

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
                _logger.LogCritical(
                    $"Error in function GetTeamRestaurantList: Date of error: {DateTime.Now} Error: {ex}");

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

        public override Task<GRPCValidationResponse> CreateOrUpdateRestaurantItem(
            GRPCCreateOrUpdateRestaurantItemRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Start of call CreateOrUpdateRestaurant: " + DateTime.Now);
            var userExistsValidationResult =
                _verifyLogin.CheckIfUsersExists(request.LoggedUser.Username, request.LoggedUser.Password,
                    Guid.Parse(request.LoggedUser.Id));

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
                _logger.LogCritical(
                    $"Error in function CreateOrUpdateRestaurant: Date of error: {DateTime.Now} Error: {ex}");

                return Task.FromResult(new GRPCValidationResponse()
                {
                    Successful = false,
                    Information = "There was an error while creating new restaurant! Please try again!",
                });
            }
        }

        public override Task<GRPCTeamRestaurnatItemsListResponse> GetTeamRestaurantItemList(
            GRPCGetInformationForGivenIdRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Start of call GetTeamRestaurantItemList: " + DateTime.Now);
            var userExistsValidationResult =
                _verifyLogin.CheckIfUsersExists(request.LoggedUser.Username, request.LoggedUser.Password,
                    Guid.Parse(request.LoggedUser.Id));

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
                _logger.LogCritical(
                    $"Error in function GetTeamRestaurantItemList: Date of error: {DateTime.Now} Error: {ex}");

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

    }
}
