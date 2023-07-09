using System.ComponentModel.DataAnnotations;
using ProcurementService.DbModels;

namespace GrpcShared.Models
{
    public class Users
    {
        public Guid Id { get; set; }
        public string? UserName { get; set; }
        public string? PasswordHash { get; set; }
        public string? SecurityStamp { get; set; }
        public int? PersonID { get; set; }
        public string? PasswordChangeToken { get; set; }
        public DateTime? PasswordChangeTokenValidDateTime { get; set; }
        public bool? PrivacyAgreed { get; set; }
        public DateTime? PrivacyAgreedOn { get; set; }
        public bool Disabled { get; set; }
        public string? VerificationCode { get; set; }
        public DateTime? VerificationCodeValidDateTime { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        public Persons Person { get; set; }

        
    }
}
