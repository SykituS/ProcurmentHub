using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcShared;
using GrpcShared.Models;
using Microsoft.EntityFrameworkCore;

namespace ProcurementService.Services
{
    public partial class ProcurementService
    {
        public override Task<GRPCOrderInformationsResponse> StartNewOrder(GRPCStartNewOrderRequest request,
            ServerCallContext context)
        {
            _logger.LogInformation("Start of call StartNewOrder: " + DateTime.Now);
            var userExistsValidationResult =
                _verifyLogin.CheckIfUsersExists(request.LoggedUser.Username, request.LoggedUser.Password,
                    Guid.Parse(request.LoggedUser.Id));

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

        public override Task<GRPCValidationResponse> AddItemsToOrder(GRPCOrderAddItems request,
            ServerCallContext context)
        {
            _logger.LogInformation("Start of call AddItemsToOrder: " + DateTime.Now);
            var userExistsValidationResult =
                _verifyLogin.CheckIfUsersExists(request.LoggedUser.Username, request.LoggedUser.Password,
                    Guid.Parse(request.LoggedUser.Id));

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

        public override Task<GRPCFullOrderDetailsResponse> GetFullOrderDetailsById(GRPCGetOrderByIdRequest request,
            ServerCallContext context)
        {
            _logger.LogInformation("Start of call GetFullOrderDetailsById: " + DateTime.Now);
            var userExistsValidationResult =
                _verifyLogin.CheckIfUsersExists(request.LoggedUser.Username, request.LoggedUser.Password,
                    Guid.Parse(request.LoggedUser.Id));

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
                            Price = order.TotalPriceOfOrder.ToString() == ""
                                ? decimal.Zero.ToString()
                                : order.TotalPriceOfOrder.ToString(),
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
                                Price = orderItem.TeamRestaurantsItems.Price.ToString() == ""
                                    ? decimal.Zero.ToString()
                                    : orderItem.TeamRestaurantsItems.Price.ToString(),
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
                _logger.LogCritical(
                    $"Error in function GetFullOrderDetailsById: Date of error: {DateTime.Now} Error: {ex}");
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

        public override Task<GRPCValidationResponse> CloseOrderById(GRPCGetOrderByIdRequest request,
            ServerCallContext context)
        {
            _logger.LogInformation("Start of call CloseOrderById: " + DateTime.Now);
            var userExistsValidationResult =
                _verifyLogin.CheckIfUsersExists(request.LoggedUser.Username, request.LoggedUser.Password,
                    Guid.Parse(request.LoggedUser.Id));

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
                order.TotalPriceOfOrder = _context.TeamOrdersItems.Where(e => e.TeamOrdersID == order.ID)
                    .Select(e => e.TotalPriceOfItem).Sum();
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

        public override Task<GRPCOrderListResponse> GetOrderListForTeamId(GRPCGetInformationForGivenIdRequest request,
            ServerCallContext context)
        {
            _logger.LogInformation("Start of call GetOrderListForTeamId: " + DateTime.Now);
            var userExistsValidationResult =
                _verifyLogin.CheckIfUsersExists(request.LoggedUser.Username, request.LoggedUser.Password,
                    Guid.Parse(request.LoggedUser.Id));

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
                            Price = order.TotalPriceOfOrder.ToString() == ""
                                ? decimal.Zero.ToString()
                                : order.TotalPriceOfOrder.ToString(),
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
                _logger.LogCritical(
                    $"Error in function GetOrderListForTeamId: Date of error: {DateTime.Now} Error: {ex}");
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

    }
}
