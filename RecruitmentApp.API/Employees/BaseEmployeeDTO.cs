using System.ComponentModel.DataAnnotations;

namespace RecruitmentApp.API.Employees;

public abstract class BaseEmployeeDTO
{
    [Required]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "First Name has to be 2-50 letters long")]
    public string FirstName { get; set; }
    
    [Required]
    [StringLength(60, MinimumLength = 2, ErrorMessage = "Last Name has to be 2-50 letters long")]
    public string LastName { get; set; }
    
    [Required]
    [StringLength(60, MinimumLength = 6, ErrorMessage = "Email Address has to be 6-60 letters long")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.(com|pl)$", ErrorMessage = "Invalid Email format")]
    public string EmailAddress { get; set; }
    
    [Required]
    public Language Language { get; set; }
    
    [Required]
    [StringLength(60, MinimumLength = 1, ErrorMessage = "Localization has to be 1-60 letters long" )]
    public string Localization { get; set; }
    
    [Required] 
    public WorkModel WorkModel { get; set; }
    
    [Required]
    [StringLength(12, MinimumLength = 9, ErrorMessage = "Phone Number has to be 9-12 numbers long")]
    public string PhoneNumber { get; set; }

    [Required]
    [Range(1,12, ErrorMessage = "Working Hours has to be a value between 1 and 12")]
    public int WorkingTimeInHours { get; set; }
    
    [Required]
    [StringLength(30)]
    public string WorkingHours { get; set; }
}