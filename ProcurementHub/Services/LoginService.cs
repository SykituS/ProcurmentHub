using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrpcShared;

namespace ProcurementHub.Services
{
    public class LoginService : BaseServices
    {
        public LoginService(Procurement.ProcurementClient procurementClient) : base(procurementClient)
        {
        }
        
    }
}
