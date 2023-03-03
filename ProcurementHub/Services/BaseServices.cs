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
        private readonly Procurement.ProcurementClient _procurementClient;

        public BaseServices(Procurement.ProcurementClient procurementClient)
        {
            _procurementClient = procurementClient;
        }
    }
}
