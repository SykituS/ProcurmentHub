using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcurementHub.ViewModel
{
    internal class TeamsMembers
    {
        int Id { get; set; }
        int TeamId { get; set; }
        int UserId { get; set; }
        int Role { get; set; }
        string Contribution { get; set; }
        string PaymentStatus { get; set; }
        
        Teams Team { get; set; }
        Users User { get; set; }
    }
}
