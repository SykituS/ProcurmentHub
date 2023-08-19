using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrpcShared;
using ProcurementHub.Model.CustomModels;
using ProcurementHub.Model.Enums;

namespace ProcurementHub.Services
{
	public class OrderServices : BaseServices
	{
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
    }
}
