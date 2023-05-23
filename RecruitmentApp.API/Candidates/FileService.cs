using Microsoft.AspNetCore.Mvc;

namespace RecruitmentApp.API.Candidates;

public class FileService : IFileService
{
    public async Task<IActionResult> SaveFile(IFormFile? file, Candidate candidate)
    {
        if (file == null)
        {
            return new BadRequestResult();
        }
        
        using (var stream = new MemoryStream())
        {
            await file.CopyToAsync(stream);
            candidate.Attachment = stream.ToArray();
        }

        return new CreatedResult("Created a candidate with pdf file", candidate);
    }

    public FileContentResult GetFile(Candidate candidate)
    {
        var candidatesAttachment = candidate.Attachment;

        const string attachmentType = "application/pdf";

        return new FileContentResult(candidatesAttachment, attachmentType)
        {
            FileDownloadName = $"CV {candidate.FirstName} {candidate.LastName}.pdf"
        };
    }
}