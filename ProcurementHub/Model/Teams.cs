namespace ProcurementHub.Model
{
    public class Teams
    {
        int ID { get; set; }
        string TeamName { get; set; }
        string Description { get; set; }
        int CreatedByID { get; set; }
        Status Status { get; set; }

        Users CreatedBy { get; set; }
    }
}
