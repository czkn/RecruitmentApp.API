using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RecruitmentApp.API.Candidates;
using RecruitmentApp.API.Employees;

namespace RecruitmentApp.API.Jobs;

public class Job
{
    public Job()
    {
        Candidates = new List<Candidate>();
        Employees = new List<Employee>();
    }
    
    public Guid Id { get; set; }

    [Required]
    [Column(TypeName = "VARCHAR")]
    [StringLength(40, MinimumLength = 5)]
    public string Name { get; set; }
    
    [Required]
    [StringLength(4000, MinimumLength = 10)]
    public string Description { get; set; }
    
    [Required]
    [StringLength(30, MinimumLength = 1)]
    public string ProgrammingLanguage { get; set; }

    [Required]
    public Language Language { get; set; }
    
    [Required]
    [StringLength(60, MinimumLength = 1)]
    public string Localization { get; set; }
    
    [Required] 
    public WorkModel WorkModel { get; set; }
    
    [Required]
    [Range( 3600, 99999999999 )]
    public decimal SalaryMin { get; set; }
    
    [Required]
    [Range( 3600, 99999999999 )]
    public decimal SalaryMax { get; set; }
    
    [Range( 1, 12 )]
    public int WorkingHours { get; set; }

    [Required]
    [StringLength(80, MinimumLength = 1)]
    public string MandatorySkills { get; set; }
    
    [StringLength(80, MinimumLength = 1)]
    public string AppreciatedSkills { get; set; }
    
    [Required]
    [StringLength(90, MinimumLength = 1)]
    public string CompanyName { get; set; }
    
    public DateTime CreationDate { get; set; }

    public IList<Candidate> Candidates { get; set; }

    public IList<Employee> Employees { get; set; }
    
    [Required]
    [ForeignKey(nameof(UserId))]
    public Guid UserId { get; set; }
}