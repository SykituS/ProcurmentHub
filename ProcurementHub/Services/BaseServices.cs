using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrpcShared;
using ProcurementHub.Model;

namespace ProcurementHub.Services
{
    public class BaseServices
    {
        protected readonly Procurement.ProcurementClient ProcurementClient;
        protected readonly ResponseMessage ResponseMessage = new();

        public BaseServices(Procurement.ProcurementClient procurementClient)
        {
            ProcurementClient = procurementClient;
        }
    }
}
