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
	}
}
