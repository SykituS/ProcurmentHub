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

		private List<TeamRestaurantsModel> _restaurants = new();
		private List<TeamRestaurantItemsModel> _restaurantItems = new();

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

		public async Task<ValidationResponseWithResult<List<TeamRestaurantsModel>>> GetRestaurantListAsync(int teamId)
		{
			var reply = await ProcurementClient.GetTeamRestaurantListAsync(new GRPCGetInformationForGivenIdRequest
			{
				LoggedUser = new GRPCLoginInformationForUser
				{
					Id = App.LoggedUserInApplication.Id.ToString(),
					Password = App.LoggedUserInApplication.PasswordHash,
					Username = App.LoggedUserInApplication.UserName,
				},
				Id = teamId
			});

			var result = new ValidationResponseWithResult<List<TeamRestaurantsModel>>();

			if (!reply.Response.Successful)
			{
				result.Successful = false;
				result.Information = reply.Response.Information;
				return result;
			}

			foreach (var restaurant in reply.RestaurantList)
			{
				_restaurants.Add(new TeamRestaurantsModel
				{
					ID = restaurant.Id,
					Name = restaurant.Name,
					Address = restaurant.Address,
					Description = restaurant.Description,
					CreatedBy = new PersonsModel
					{
						Id = restaurant.CreatedBy.Id,
						FirstName = restaurant.CreatedBy.FirstName,
						LastName = restaurant.CreatedBy.LastName,
						Email = restaurant.CreatedBy.Email,
						//FullName = $"{restaurant.CreatedBy.FirstName} {restaurant.CreatedBy.LastName}",
					},
					IsUpdated = restaurant.CreatedOn != restaurant.UpdatedOn,
					UpdatedBy = new PersonsModel
					{
						Id = restaurant.UpdatedBy.Id,
						FirstName = restaurant.UpdatedBy.FirstName,
						LastName = restaurant.UpdatedBy.LastName,
						Email = restaurant.UpdatedBy.Email,
						//FullName = $"{restaurant.UpdatedBy.FirstName} {restaurant.UpdatedBy.LastName}",
					},
				});
			}

			result.ResultValues = _restaurants;
			result.Successful = true;
			return result;
		}

		public async Task<ValidationResponse> CreateOrUpdateRestaurantItemAsync(TeamRestaurantItemsModel teamRestaurantsModel, int teamRestaurantId)
		{
			var reply = await ProcurementClient.CreateOrUpdateRestaurantItemAsync(new GRPCCreateOrUpdateRestaurantItemRequest()
			{
				Id = teamRestaurantsModel.ID,
				RestaurantId = teamRestaurantId,
				Name = teamRestaurantsModel.Name,
				Price = new money()
				{
					CurrencyCode = teamRestaurantsModel.CurrencyType,
					Price = teamRestaurantsModel.Price.ToString(),
				},
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

		public async Task<ValidationResponseWithResult<List<TeamRestaurantItemsModel>>> GetRestaurantItemsListAsync(int restaurantId)
		{
			var reply = await ProcurementClient.GetTeamRestaurantItemListAsync(new GRPCGetInformationForGivenIdRequest
			{
				LoggedUser = new GRPCLoginInformationForUser
				{
					Id = App.LoggedUserInApplication.Id.ToString(),
					Password = App.LoggedUserInApplication.PasswordHash,
					Username = App.LoggedUserInApplication.UserName,
				},
				Id = restaurantId
			});

			var result = new ValidationResponseWithResult<List<TeamRestaurantItemsModel>>();

			if (!reply.Response.Successful)
			{
				result.Successful = false;
				result.Information = reply.Response.Information;
				return result;
			}

			foreach (var item in reply.RestaurantItemList)
			{
				_restaurantItems.Add(new TeamRestaurantItemsModel
				{
					ID = item.Id,
					Name = item.Name,
					Price = decimal.Parse(item.Price.Price),
					CurrencyType = item.Price.CurrencyCode,
					Description = item.Description,
					CreatedBy = new PersonsModel
					{
						Id = item.CreatedBy.Id,
						FirstName = item.CreatedBy.FirstName,
						LastName = item.CreatedBy.LastName,
						Email = item.CreatedBy.Email,
						//FullName = $"{item.CreatedBy.FirstName} {item.CreatedBy.LastName}",
					},
					IsUpdated = item.CreatedOn != item.UpdatedOn,
					UpdatedBy = new PersonsModel
					{
						Id = item.UpdatedBy.Id,
						FirstName = item.UpdatedBy.FirstName,
						LastName = item.UpdatedBy.LastName,
						Email = item.UpdatedBy.Email,
						//FullName = $"{item.UpdatedBy.FirstName} {item.UpdatedBy.LastName}",
					},
				});
			}

			result.ResultValues = _restaurantItems;
			result.Successful = true;
			return result;
		}
	}
}
