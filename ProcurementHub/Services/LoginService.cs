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

        public async Task<ResponseMessage> LoginUserAsync(LoginUser user)
        {
            var response = await ProcurementClient.LoginUserAsync(new GRPCLoginUser
            {
                Email = user.UserName,
                Password = user.Password,
            });

            ResponseMessage.Code = response.Code;
            ResponseMessage.Message = response.Message;

            return ResponseMessage;
        }

        public async Task<Users> GetUserDataAsync(Guid guid)
        {
            var response = await ProcurementClient.GetUserDataAsync(new GRPCStatus() { Message = guid.ToString() });

            var mapper = MapperConfig.InitializeAutomapper();

            var result = mapper.Map<GRPCLoggedUser, Users>(response);

            return result;
        }

    }
}
