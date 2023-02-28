namespace ProcurementHub.Domain.Models
{
    public class Teams
    {
        public int ID { get; set; }
        public string TeamName { get; set; }
        public string Description { get; set; }
        public int CreatedByID { get; set; }
        public Status Status { get; set; }
         
        public Users CreatedBy { get; set; }
    }
}
