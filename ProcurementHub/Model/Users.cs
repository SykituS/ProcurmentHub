namespace ProcurementHub.Model
{
    public class Users
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRoles Role { get; set; }
        public string Status { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedDate { get; set; }
         
        public string VerificationCode { get; set; }
        public string VerificationStatus { get; set; }
         
        public string ResetCode { get; set; }
        public DateTime ResetCodeExpiry { get; set; }
        
        public bool IsDeleted { get; set; }
        public DateTime DeletedDate { get; set; }

    }

    public enum UserRoles
    {
        Admin,
        User
    }

    public enum UserStatus
    {
        Active,
        Inactive
    }

    public enum VerificationStatus
    {
        Verified,
        Pending,
        Unverified
    }
}
