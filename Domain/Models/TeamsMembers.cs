namespace ProcurementHub.Domain.Models
{
    public class TeamsMembers
    {
        int ID { get; set; }
        int TeamID { get; set; }
        int UserID { get; set; }
        TeamRoles Role { get; set; }
        
        Teams Team { get; set; }
        ICollection<Users> User { get; set; }
    }

    enum TeamRoles
    {
        Creator,
        Member
    }
}
