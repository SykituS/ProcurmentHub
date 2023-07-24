using ProcurementHub.Model.Enums;

namespace ProcurementHub.Model.Models
{
    public class Teams
    {
        public int ID { get; set; }
        public string TeamName { get; set; }
        public string Description { get; set; }
        public TeamStatusEnum Status { get; set; }
        public string TeamJoinCode { get; set; }
        public string TeamJoinPassword { get; set; }
        public int CreatedById { get; set; }
        public DateTime CreatedOn { get; set; }
        public int UpdatedById { get; set; }
        public DateTime UpdatedOn { get; set;}

        public Persons CreatedBy { get; set; }
        public Persons UpdatedBy { get; set; }
    }

    
}
