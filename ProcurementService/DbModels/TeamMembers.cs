using ProcurementService.DbModels;

namespace GrpcShared.Models
{
    public class TeamMembers
    {
        public int ID { get; set; }
        public int TeamID { get; set; }
        public int PersonID { get; set; }
        public TeamRoleEnum Role { get; set; }

        public Teams Team { get; set; }
        public Persons Person { get; set; }
    }

    public enum TeamRoleEnum
    {
        TeamMember,
        TeamAdministrator,
    }
}
