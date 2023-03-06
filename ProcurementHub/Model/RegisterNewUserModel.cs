using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrpcShared.Models;

namespace ProcurementHub.Model
{
    public class RegisterNewUserModel
    {
        public Persons Person { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
