using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcurementHub.ViewModel
{
    internal class Users
    {
        int ID { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Email { get; set; }
        string Password { get; set; }
        string Phone { get; set; }
        string Address { get; set; }
        string Role { get; set; }
        string Status { get; set; }
        string CreatedDate { get; set; }
        string UpdatedDate { get; set; }

        string VerificationCode { get; set; }
        string VerificationStatus { get; set; }

        string ResetCode { get; set; }
        DateTime ResetCodeExpiry { get; set; }

        bool IsDeleted { get; set; }
        DateTime DeletedDate { get; set; }

    }
}
