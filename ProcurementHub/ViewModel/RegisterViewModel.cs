using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrpcShared;

namespace ProcurementHub.ViewModel
{
    public partial class RegisterViewModel : BaseViewModel
    {
        public RegisterViewModel(Procurement.ProcurementClient procurementClient) : base(procurementClient)
        {
            Title = "Register";
        }
    }
}
