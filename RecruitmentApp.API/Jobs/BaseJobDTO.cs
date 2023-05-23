using System.ComponentModel.DataAnnotations;
using NuGet.Protocol.Plugins;

namespace RecruitmentApp.API.Jobs;

public abstract class BaseJobDTO : IValidatableObject
{
    [Required]
    [StringLength(40, MinimumLength = 5, ErrorMessage = "Name has to be 5-40 letters long")]
    public string Name { get; set; }
    
    [Required]
    [StringLength(4000, MinimumLength = 10, ErrorMessage = "Description has to be 10-4000 letters long")]
    public string Description { get; set; }
    
    [Required]
    [StringLength(30, MinimumLength = 1, ErrorMessage = "Programming Language has to be 1-30 letters long")]
    public string ProgrammingLanguage { get; set; }

    [Required]
    public Language Language { get; set; }
    
    [Required]
    [StringLength(60, MinimumLength = 1, ErrorMessage = "Localization has to be 1-60 letters long")]
    public string Localization { get; set; }
    
    [Required] 
    public WorkModel WorkModel { get; set; }
    
    [Required]
    [Range( 3600, 99999999999, ErrorMessage = "Salary Min has to be a value between 3600 and 99999999999")]
    public decimal SalaryMin { get; set; }

    [Required] 
    [Range(3600, 99999999999, ErrorMessage = "Salary Max has to be a value between 3600 and 99999999999")]
    public decimal SalaryMax { get; set; } 
    
    [Range( 1, 12, ErrorMessage = "Working Hours has to be a value between 1 and 12")]
    public int WorkingHours { get; set; }

    [Required]
    [StringLength(80, MinimumLength = 1, ErrorMessage = "Mandatory Skills has to be 1-80 letters long")]
    public string MandatorySkills { get; set; }
    
    [StringLength(80, MinimumLength = 1, ErrorMessage = "Appreciated Skills has to be 1-80 letters long")]
    public string AppreciatedSkills { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (SalaryMax <= SalaryMin)
        {
            yield return new ValidationResult("SalaryMax must be greater than SalaryMin.", new[] { "SalaryMax", "SalaryMin" });
        }
    }
}