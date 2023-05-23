using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Store.PartnerCenter.Models.Users;

namespace RecruitmentApp.API.Candidates;

public class Candidate
{
    public Guid Id { get; set; }
    
    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string FirstName { get; set; }
    
    [Required]
    [StringLength(60, MinimumLength = 2)]
    public string LastName { get; set; }

    [Required]
    [StringLength(60, MinimumLength = 6)]
    public string EmailAddress { get; set; }

    [Required]
    public Language Language { get; set; }

    [Required]
    [Column(TypeName = "VARCHAR")]
    [StringLength(12, MinimumLength = 9)]
    public string PhoneNumber { get; set; }
    
    [Required]
    [ForeignKey(nameof(JobId))]
    public Guid JobId { get; set; }
    
    [Required]
    [ForeignKey(nameof(UserId))]
    public Guid UserId { get; set; }

    [Required]
    public byte[] Attachment { get; set; }

}