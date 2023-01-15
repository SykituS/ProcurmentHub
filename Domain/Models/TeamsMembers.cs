namespace ProcurementHub.Domain.Models
{
    public class TeamsMembers
    {
        public int ID { get; set; }
        public int TeamID { get; set; }
        public int UserID { get; set; }
        public TeamRoles Role { get; set; }
         
        public Teams Team { get; set; }
        public Users User { get; set; }
    }

    public enum TeamRoles
    {
        Creator,
        Member
    }
}
