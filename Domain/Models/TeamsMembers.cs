﻿namespace ProcurementHub.Domain.Models
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
