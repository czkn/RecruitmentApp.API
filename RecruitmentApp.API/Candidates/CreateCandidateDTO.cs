using System.ComponentModel.DataAnnotations;

namespace RecruitmentApp.API.Candidates;

public class CreateCandidateDTO : BaseCandidateDTO
{
    [Required]
    public Guid JobId { get; set; }
}