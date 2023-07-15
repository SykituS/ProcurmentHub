using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrpcShared;
using ProcurementHub.Model.CustomModels;

namespace ProcurementHub.Services
{
    public class TeamRestaurantsService : BaseServices
	{
		public TeamRestaurantsService(Procurement.ProcurementClient procurementClient) : base(procurementClient)
		{
		}

		public async Task<ValidationResponse> CreateOrUpdateRestaurantAsync(TeamRestaurantsModel teamRestaurantsModel, int teamId)
		{
			var reply = await ProcurementClient.CreateOrUpdateRestaurantAsync(new GRPCCreateOrUpdateRestaurantRequest
			{
				Id = teamRestaurantsModel.ID,
				TeamId = teamId,
				Name = teamRestaurantsModel.Name,
				Address = teamRestaurantsModel.Address,
				Description = teamRestaurantsModel.Description ?? "",
				LoggedUser = new GRPCLoginInformationForUser
				{
					Id = App.LoggedUserInApplication.Id.ToString(),
					Password = App.LoggedUserInApplication.PasswordHash,
					Username = App.LoggedUserInApplication.UserName,
				},
				IsDeleted = teamRestaurantsModel.IsDeleted,

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
