using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrpcShared;

namespace ProcurementHub.Services
{
    public class BaseServices
    {
        public Procurement.ProcurementClient ProcurementClient;

        public BaseServices(Procurement.ProcurementClient procurementClient)
        {
            ProcurementClient = procurementClient;
        }
    }
}
