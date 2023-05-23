using System.ComponentModel.DataAnnotations;

namespace RecruitmentApp.API.Candidates;

public class GetCandidateDTO : BaseCandidateDTO
{ 
    public Guid Id { get; set; }
    
    public Guid JobId { get; set; }
    
}