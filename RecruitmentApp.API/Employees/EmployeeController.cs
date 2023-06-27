using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace RecruitmentApp.API.Employees;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = "HrPolicy")]
public class EmployeeController : ControllerBase
{
    private readonly RecruitmentAppDbContext _context;

    public EmployeeController(RecruitmentAppDbContext context)
    {
        _context = context;
    }
    
    [HttpPost("{jobId}, {candidateId}")]
    public async Task<ActionResult<Employee>> PostEmployee(CreateEmployeeDTO createEmployeeDto, Guid jobId, Guid candidateId)
    {
        if (!ModelState.IsValid) 
            return BadRequest("The model state is not valid");
        
        var employee = createEmployeeDto.CreateEmployeeDtoToEmployee();
        
        if (!employee.EmailAddress.IsValidEmail())
            return BadRequest("Email is not valid");

        if (!employee.PhoneNumber.IsValidPhoneNumber())
            return BadRequest("Phone Number is not valid");
        
        var job = await _context.Jobs.FindAsync(jobId);
    
        if (job == null)
        {
            return NotFound();
        }

        var candidate = await _context.Candidates.FindAsync(candidateId);

        if (candidate == null)
        {
            return NotFound();
        }

        if (candidate.JobId != job.Id)
        {
            return Unauthorized("You can not add an Employee when there is no Candidate for this Job");
        }
        
        var userEmailClaim = User.FindFirst(ClaimTypes.Email)?.Value;

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmailClaim);
        
        if (user == null)
        {
            return NotFound();
        }

        if (job.UserId != user.Id)
        {
            return Unauthorized("You can not accept a Candidate that is not applying for your Job Offer");
        }

        employee.Salary = job.SalaryMin + " - " + job.SalaryMax;
        employee.UserId = user.Id;
        employee.JobId = job.Id;
        
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        return Created("Created an Employee", createEmployeeDto);
    }
}