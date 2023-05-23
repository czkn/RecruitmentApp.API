using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecruitmentApp.API.Candidates;

public abstract class BaseCandidateDTO
{
    [Required]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "First Name has to be 2-50 letters long")]
    public string FirstName { get; set; }
    
    [Required]
    [StringLength(60, MinimumLength = 2, ErrorMessage = "Last Name has to be 2-60 letters long")]
    public string LastName { get; set; }

    [Required]
    [StringLength(60, MinimumLength = 6, ErrorMessage = "Email has to be 6-60 letters long")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.(com|pl)$", ErrorMessage = "Invalid Email format")]
    public string EmailAddress { get; set; }
    
    [Required]
    public Language Language { get; set; }
    
    [Required]
    [StringLength(12, MinimumLength = 9, ErrorMessage = "Phone Number has to be 9-12 numbers long")]
    public string PhoneNumber { get; set; }
}