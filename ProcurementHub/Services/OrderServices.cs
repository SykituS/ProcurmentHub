using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GrpcShared;
using ProcurementHub.Infrastructure;
using ProcurementHub.Model.CustomModels;
using ProcurementHub.Model.Enums;

namespace ProcurementHub.Services
{
	public class OrderServices : BaseServices
	{
		private IMapper _mapper = MapperConfig.CreateMapper();
		public OrderServices(Procurement.ProcurementClient procurementClient) : base(procurementClient)
        {
        }

		public async Task<ValidationResponseWithResult<OrderModel>> StartNewOrder(int teamId, int restaurantId)
		{
			var reply = await ProcurementClient.StartNewOrderAsync(new GRPCStartNewOrderRequest
			{
				TeamId = teamId,
				RestaurantId = restaurantId,
				LoggedUser = new GRPCLoginInformationForUser
				{
					Id = App.LoggedUserInApplication.Id.ToString(),
					Username = App.LoggedUserInApplication.UserName,
					Password = App.LoggedUserInApplication.PasswordHash
				}
			});
			var result = new ValidationResponseWithResult<OrderModel>();

			if (!reply.Response.Successful)
			{
				result.Successful = false;
				result.Information = reply.Response.Information;
				return result;
			}

            result.Successful = true;
            result.ResultValues = new OrderModel
            {
	            ID = Guid.Parse(reply.Order.Id),
	            TeamID = reply.Order.TeamId,
	            TeamRestaurantID = reply.Order.RestaurantId,
	            Status = (TeamOrderStatusEnum)reply.Order.Status,
	            OrderStartedByID = reply.Order.OrderStartedBy,
	            OrderStartedOn = reply.Order.StartedOn.ToDateTime(),
				OrderPayedByID = reply.Order.OrderPayedById,
            };

			return result;
		}

        public async Task<ValidationResponse> AddItemsToOrder(List<OrderItemsModel> orderItems, Guid orderId, bool userWantToFinish)
        {
            var result = new ValidationResponseWithResult<OrderModel>();

	        var request = new GRPCOrderAddItems()
	        {
				OrderId = orderId.ToString(),
				UserWantToFinish = userWantToFinish,
				LoggedUser = new GRPCLoginInformationForUser
				{
					Id = App.LoggedUserInApplication.Id.ToString(),
					Username = App.LoggedUserInApplication.UserName,
					Password = App.LoggedUserInApplication.PasswordHash
				}
			};

	        foreach (var item in orderItems)
	        {
		        request.Items.Add(new GRPCOrderItem
		        {
			        TeamOrderId = item.TeamOrdersID.ToString(),
			        TeamRestaurantsItemId = item.TeamRestaurantsItemID,
			        Quantity = item.Quantity,
			        TotalPriceOfItem = new money()
			        {
				        CurrencyCode = "PLN",
				        Price = item.TotalPriceOfItem.ToString()
			        },
			        DivideToken = item.DivideToken.ToString(),
			        DivideOnNumberOfPersons = item.DivideOnNumberOfPersons??0,
			        DividePrice = new money()
			        {
				        CurrencyCode = "PLN",
				        Price = item.DividedPrice.ToString()
					}
		        });
	        }

	        var reply = await ProcurementClient.AddItemsToOrderAsync(request);

			result.Successful = reply.Successful;
			result.Information = reply.Information;

			return result;
        }

        public async Task<(ValidationResponse, OrderModel, List<OrderItemsModel>)> GetFullOrderDetails(Guid orderID)
        {
            var validationResponse = new ValidationResponse();
			var orderDetails = new OrderModel();
            var orderItems = new List<OrderItemsModel>();

            var reply = await ProcurementClient.GetFullOrderDetailsByIdAsync(new GRPCGetOrderByIdRequest
            {
                OrderId = orderID.ToString(),
                LoggedUser = new GRPCLoginInformationForUser
                {
                    Id = App.LoggedUserInApplication.Id.ToString(),
                    Username = App.LoggedUserInApplication.UserName,
                    Password = App.LoggedUserInApplication.PasswordHash
                }
            });

            if (!reply.Response.Successful)
            {
                validationResponse.Information = reply.Response.Information;
				validationResponse.Successful = false;
			    return (validationResponse, orderDetails, orderItems);
            }

            var orderReply = reply.Order;
			orderDetails = _mapper.Map<OrderModel>(orderReply);

            foreach (var item in reply.Items)
            {
                orderItems.Add(_mapper.Map<OrderItemsModel>(item));
            }

            validationResponse.Successful = true;

            return (validationResponse, orderDetails, orderItems);
        }

        public async Task<ValidationResponse> CloseOrder(Guid orderID)
        {
            var reply = await ProcurementClient.CloseOrderByIdAsync(new GRPCGetOrderByIdRequest
            {
                OrderId = orderID.ToString(),
                LoggedUser = new GRPCLoginInformationForUser
                {
                    Id = App.LoggedUserInApplication.Id.ToString(),
                    Username = App.LoggedUserInApplication.UserName,
                    Password = App.LoggedUserInApplication.PasswordHash
                }
            });

			var response = new ValidationResponse
            {
                Successful = reply.Successful,
                Information = reply.Information
            };

            return response;
        }
    }
}
