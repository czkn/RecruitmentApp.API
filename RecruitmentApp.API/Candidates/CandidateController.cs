using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace RecruitmentApp.API.Candidates
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "HrPolicy")]
    public class CandidateController : ControllerBase
    {
        private readonly RecruitmentAppDbContext _context;
        private readonly IFileService _fileService;

        public CandidateController(RecruitmentAppDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<GetCandidateDTO>>> GetCandidates(Guid userId)
        {
            var candidates = await _context.Candidates.ToListAsync();

            var candidatesAssignedToHrUser = candidates.Where(c => c.UserId == userId).ToList();

            var candidatesDto = candidatesAssignedToHrUser.Select(c => c.CandidateToGetCandidateDto()).ToList();
            
            return candidatesDto;
        }

        [HttpGet("{candidateId}/GetCandidateCv,{userId}")]
        public async Task<IActionResult> GetCandidateCv(Guid candidateId, Guid userId)
        {
            var candidate = _context.Candidates
                .FirstOrDefault(c => c.Id == candidateId);

            if (candidate == null)
            {
                return NotFound();
            }
            
            var user = await _context.Users.FindAsync(userId);
            
            if (user == null)
            {
                return NotFound();
            }
            
            if (candidate.UserId != user.Id)
            {
                return Unauthorized("You can not get a candidate CV that is applying to a Job created by other Hr");
            }
            
            var cv = _fileService.GetFile(candidate);

            return cv;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<Candidate>> PostCandidate([FromForm] CreateCandidateDTO createCandidateDto, IFormFile file)
        {
            if (!ModelState.IsValid) 
                return BadRequest("The model state is not valid");
            
            if (file.FileName.Substring(file.FileName.Length - 3) != "pdf")
                return BadRequest("File is not a PDF.");

            var candidate = createCandidateDto.CreateCandidateDtoToCandidate();
                
            if (!candidate.EmailAddress.IsValidEmail())
                return BadRequest("Email is not valid");
            
            if (!candidate.PhoneNumber.IsValidPhoneNumber())
                return BadRequest("Phone Number is not valid");
            
            var job = await _context.Jobs.FindAsync(candidate.JobId);
    
            if (job == null)
            {
                return NotFound();
            }

            candidate.UserId = job.UserId;
                
            await _fileService.SaveFile(file, candidate);
                
            _context.Candidates.Add(candidate);
            await _context.SaveChangesAsync();

            return Created("Created a Candidate", createCandidateDto);
        }

        [HttpDelete("{candidateId}, {userId}")]
        public async Task<ActionResult> DeleteCandidate(Guid candidateId, Guid userId)
        {
            var candidate = await _context.Candidates.FindAsync(candidateId);
            
            if (candidate == null)
            {
                return NotFound();
            }
            
            var user = await _context.Users.FindAsync(userId);
    
            if (user == null)
            {
                return NotFound();
            }

            if (candidate.UserId != user.Id)
            {
                return Unauthorized("You can not delete a candidate that is applying to a Job created by other Hr");
            }

            _context.Candidates.Remove(candidate);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
