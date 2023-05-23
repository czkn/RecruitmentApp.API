using System.ComponentModel.DataAnnotations;
using Microsoft.Build.Framework;

namespace RecruitmentApp.API.Jobs;

public class GetJobDTO : BaseJobDTO
{
    public Guid Id { get; set; }
    
    [StringLength(90, MinimumLength = 1)]
    public string CompanyName { get; set; }
}