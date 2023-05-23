using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RecruitmentApp.API.Candidates;
using RecruitmentApp.API.Employees;
using RecruitmentApp.API.Jobs;
using RecruitmentApp.API.Users;

namespace RecruitmentApp.API;

public class RecruitmentAppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public RecruitmentAppDbContext(DbContextOptions options) : base(options)
    {
       
    }

    public DbSet<Candidate> Candidates { get; set; }

    public DbSet<Job> Jobs { get; set; }

    public DbSet<Employee> Employees { get; set; }
}