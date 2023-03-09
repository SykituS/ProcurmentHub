using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrpcShared;
using GrpcShared.Models;
using ProcurementHub.Infrastructure;
using ProcurementHub.Model;

namespace ProcurementHub.Services
{
    public class LoginService : BaseServices
    {
        public LoginService(Procurement.ProcurementClient procurementClient) : base(procurementClient)
        {
        }

        public async Task<GRPCLoggedUser> LoginUserAsync(LoginUser user)
        {
            var response = await ProcurementClient.LoginUserToApplicationAsync(new GRPCLoginUser
            {
                Email = user.UserName,
                Password = user.Password,
            });

            return response;
        }

        public async Task<Users> ConvertRequestToUserData(GRPCLoggedUser request)
        {

            //var mapper = MapperConfig.InitializeAutomapper();
            //var result = mapper.Map<GRPCLoggedUser, Users>(response);

            var result = new Users
            {
                Id = Guid.Parse(request.User.Id),
                UserName = request.User.UserName,
                PasswordHash = request.User.PasswordHash,
                SecurityStamp = request.User.SecurityStamp,
                PersonID = request.User.PersonID,
                PrivacyAgreed = request.User.PrivacyAgreed,
                PrivacyAgreedOn = request.User.PrivacyAgreedOn.Length > 0 ? DateTime.Parse(request.User.PrivacyAgreedOn) : null,
                Disabled = request.User.Disabled,
                CreatedOn = DateTime.Parse(request.User.CreatedOn),
                UpdatedOn = DateTime.Parse(request.User.UpdatedOn),
                Person = new Persons
                {
                    Id = request.Person.Id,
                    FirstName = request.Person.FirstName,
                    LastName = request.Person.LastName,
                    Email = request.Person.Email,
                }
            };

            return result;
        }

    }
}
