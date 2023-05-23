using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using RecruitmentApp.API.Candidates;
using RecruitmentApp.API.Employees;
using RecruitmentApp.API.Jobs;

namespace RecruitmentApp.API.Users;

public class User : IdentityUser<Guid>
{
    [Required]
    [StringLength(30, MinimumLength = 1)]
    public string FirstName { get; set; }
    
    [Required]
    [StringLength(60, MinimumLength = 1)]
    public string LastName { get; set; }
    
    [Required]
    [StringLength(90, MinimumLength = 1)]
    public string CompanyName { get; set; }
    
    public IList<Job> Jobs { get; set; }
    
    public IList<Candidate> Candidates { get; set; }
    
    public IList<Employee> Employees { get; set; }
}