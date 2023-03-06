using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrpcShared;
using ProcurementHub.Model;

namespace ProcurementHub.Services
{
    public class RegisterServices : BaseServices
    {
        public RegisterServices(Procurement.ProcurementClient procurementClient) : base(procurementClient)
        {
        }
        
        public async Task<ResponseMessage> RegisterNewUserAsync(RegisterNewUserModel model)
        {
            var result = await ProcurementClient.RegisterUserAsync(new GRPCRegisterNewUser
            {
                Password = model.Password,
                ConfirmPassword = model.ConfirmPassword,
                Person = new GRPCPerson
                {
                    FirstName = model.Person.FirstName,
                    LastName = model.Person.LastName,
                    Email = model.Person.Email,
                }
            });

            ResponseMessage.Code = result.Code;
            ResponseMessage.Message = result.Message;

            return ResponseMessage;
        }
    }
}
