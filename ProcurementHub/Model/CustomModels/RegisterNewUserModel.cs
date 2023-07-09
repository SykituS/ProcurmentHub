using ProcurementHub.Model.Models;

namespace ProcurementHub.Model.CustomModels
{
    public class RegisterNewUserModel
    {
        public Persons Person { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
