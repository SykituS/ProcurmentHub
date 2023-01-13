namespace ProcurementHub.Domain.Models
{
    public class Teams
    {
        int Id { get; set; }
        int TeamName { get; set; }
        int Description { get; set; }
        int CreatorId { get; set; }
        int Status { get; set; }
        Users Creator { get; set; }
    }
}
