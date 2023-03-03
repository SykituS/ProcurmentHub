using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrpcShared;

namespace ProcurementHub.Services
{
    public class RegisterServices : BaseServices
    {
        public RegisterServices(Procurement.ProcurementClient procurementClient) : base(procurementClient)
        {
        }
    }
}
