﻿
using ProcurementHub.Model.Enums;

namespace ProcurementHub.Model.Models
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

    
}
