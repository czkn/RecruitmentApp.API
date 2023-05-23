using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecruitmentApp.API.Employees;

public class Employee
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
    [StringLength(60, MinimumLength = 1)]
    public string Localization { get; set; }
    
    [Required] 
    public WorkModel WorkModel { get; set; }
    
    [Required]
    [Column(TypeName = "VARCHAR")]
    [StringLength(12, MinimumLength = 9)]
    public string PhoneNumber { get; set; }
    
    [StringLength(1000, MinimumLength = 8)]
    public string Salary { get; set; }
    
    [Required]
    [Range(1,12)]
    public int WorkingTimeInHours { get; set; }
    
    [Column(TypeName = "VARCHAR")]
    [Required]
    [StringLength(30)]
    public string WorkingHours { get; set; }
    
    [Required]
    [ForeignKey(nameof(JobId))]
    public Guid JobId { get; set; }
    
    [Required]
    [ForeignKey(nameof(UserId))]
    public Guid UserId { get; set; }
}