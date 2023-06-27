using System.Security.Claims;
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCandidateDTO>>> GetCandidates()
        {
            var candidates = await _context.Candidates.ToListAsync();
            
            var userEmailClaim = User.FindFirst(ClaimTypes.Email)?.Value;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmailClaim);

            var candidatesAssignedToHrUser = candidates.Where(c => c.UserId == user?.Id).ToList();

            var candidatesDto = candidatesAssignedToHrUser.Select(c => c.CandidateToGetCandidateDto()).ToList();
            
            return candidatesDto;
        }

        [HttpGet("{candidateId}/GetCandidateCv")]
        public async Task<IActionResult> GetCandidateCv(Guid candidateId)
        {
            var candidate = _context.Candidates
                .FirstOrDefault(c => c.Id == candidateId);

            if (candidate == null)
            {
                return NotFound();
            }
            
            var userEmailClaim = User.FindFirst(ClaimTypes.Email)?.Value;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmailClaim);
            
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

        [HttpDelete("{candidateId}")]
        public async Task<ActionResult> DeleteCandidate(Guid candidateId)
        {
            var candidate = await _context.Candidates.FindAsync(candidateId);
            
            if (candidate == null)
            {
                return NotFound();
            }
            
            var userEmailClaim = User.FindFirst(ClaimTypes.Email)?.Value;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmailClaim);
    
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
