namespace RecruitmentApp.API.Candidates;

public static class CandidateMapper
{
    public static Candidate CreateCandidateDtoToCandidate(this CreateCandidateDTO createCandidateDto)
    {
        return new Candidate
        {
            FirstName = createCandidateDto.FirstName,
            LastName = createCandidateDto.LastName,
            Language = createCandidateDto.Language,
            EmailAddress = createCandidateDto.EmailAddress,
            PhoneNumber = createCandidateDto.PhoneNumber,
            JobId = createCandidateDto.JobId
        };
    }

    public static GetCandidateDTO CandidateToGetCandidateDto(this Candidate candidate)
    {
        return new GetCandidateDTO
        {
            Id = candidate.Id,
            FirstName = candidate.FirstName,
            LastName = candidate.LastName,
            Language = candidate.Language,
            EmailAddress = candidate.EmailAddress,
            PhoneNumber = candidate.PhoneNumber,
            JobId = candidate.JobId
        };
    }
}