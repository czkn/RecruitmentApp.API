using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Store.PartnerCenter.Models.Users;

namespace RecruitmentApp.API.Jobs;

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "HrPolicy")]
    public class JobController : ControllerBase
    {
        private readonly RecruitmentAppDbContext _context;

        public JobController(RecruitmentAppDbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<GetJobDTO>>> GetJobs()
        {
            var jobs = await _context.Jobs
                .Include(c => c.Candidates)
                .Include(e => e.Employees)
                .ToListAsync();

            var jobsSortedByCreationDate = jobs.OrderByDescending(j => j.CreationDate).ToList();

            return jobsSortedByCreationDate.Select(j => j.JobToGetJobDto()).ToList();
        }
        
        [HttpGet]
        [Route("CreatedByHrUser")]
        public async Task<ActionResult<IEnumerable<GetJobDTO>>> GetJobsCreatedByHrUser()
        {
            var jobs = await _context.Jobs
                .Include(c => c.Candidates)
                .Include(e => e.Employees)
                .ToListAsync();
            
            var userEmailClaim = User.FindFirst(ClaimTypes.Email)?.Value;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmailClaim);

            var jobsCreatedByHrUser = jobs.Where(j => j.UserId == user?.Id).ToList();
            
            var jobsSortedByCreationDate = jobsCreatedByHrUser.OrderByDescending(j => j.CreationDate).ToList();

            var jobsDto = jobsSortedByCreationDate.Select(j => j.JobToGetJobDto()).ToList();
            
            return jobsDto;
        }

        [HttpPost]
        public async Task<ActionResult<Job>> PostJob(CreateJobDTO createJobDto)
        {
            if (!ModelState.IsValid) 
                return BadRequest("The model state is not valid");
            
            var job = createJobDto.CreateJobDtoToJob();
            
            var userEmailClaim = User.FindFirst(ClaimTypes.Email)?.Value;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmailClaim);

            if (user == null)
            {
                return NotFound();
            }

            job.CreationDate = DateTime.Now;
            job.CompanyName = user.CompanyName;
            job.UserId = user.Id;
            
            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();

            return Created("Created a Job", createJobDto);
        }
        
        [HttpDelete("{jobId}")]
        public async Task<IActionResult> DeleteJob(Guid jobId)
        {
            var job = await _context.Jobs.FindAsync(jobId);
    
            if (job == null)
            {
                return NotFound();
            }

            var userEmailClaim = User.FindFirst(ClaimTypes.Email)?.Value;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmailClaim);
    
            if (user == null)
            {
                return NotFound();
            }

            if (job.UserId != user.Id)
            {
                return Unauthorized("You can not delete a job that was created by other Hr");
            }

            _context.Jobs.Remove(job);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

