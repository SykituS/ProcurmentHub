using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcurementHub.Model
{
    public class TeamsMembers
    {
        int Id { get; set; }
        int TeamId { get; set; }
        TeamRoles Role { get; set; }
        string Contribution { get; set; }
        string PaymentStatus { get; set; }

        Teams Team { get; set; }
        ICollection<Users> Users { get; set; }
    }

    enum TeamRoles
    {
        Creator,
        Member
    }
}
