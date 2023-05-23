using Microsoft.AspNetCore.Mvc;

namespace RecruitmentApp.API.Candidates;

public interface IFileService
{
    Task<IActionResult> SaveFile(IFormFile file, Candidate candidate);

    FileContentResult GetFile(Candidate candidate);
}