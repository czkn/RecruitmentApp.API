using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RecruitmentApp.API.Candidates;
using RecruitmentApp.API.Jobs;

namespace RecruitmentApp.API.Tests;

public class AppFactory<T> : WebApplicationFactory<Program> where T : Program
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<RecruitmentAppDbContext>));

            if (descriptor != null)
                services.Remove(descriptor);

            services.AddDbContext<RecruitmentAppDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryRecruitmentAppDb");
                options.EnableSensitiveDataLogging();
            });

            var serviceProvider = services.BuildServiceProvider();
            
            var scope = serviceProvider.CreateScope();
            
            var appContext = scope.ServiceProvider.GetRequiredService<RecruitmentAppDbContext>();
             
            try
            {
                appContext.Database.EnsureCreated();
            }
            catch (Exception exception)
            {
                throw;
            }

            appContext.Add(new Job
            {
                Id = new Guid("5dcd897b-4b68-4346-8f96-943b27d5a8ac"),
                Name = "Java Developer",
                Description = "Description of Java Developer",
                ProgrammingLanguage = "Java",
                Language = Language.English,
                Localization = "Rome",
                WorkModel = (WorkModel)0,
                SalaryMin = 3500,
                SalaryMax = 10000,
                WorkingHours = 8,
                MandatorySkills = "Project Patterns, Git",
                AppreciatedSkills = "SQl",
                CompanyName = "BB-IT"
            });

            appContext.Add(new Candidate
            {
                Id = new Guid("157439c5-d908-47ff-a648-f9ac059f2ea2"),
                FirstName = "Jack",
                LastName = "Newton",
                EmailAddress = "jnewton@gmail.com",
                Language = Language.English,
                PhoneNumber = "903 894 983",
                JobId = new Guid("5dcd897b-4b68-4346-8f96-943b27d5a8ac"),
                Attachment = new byte[] {byte.MinValue, byte.MaxValue  }
            });
            
            appContext.Add(new Candidate
            {
                Id = new Guid("95d4647c-470b-408b-b5f9-d405a4ed707e"),
                FirstName = "Leon",
                LastName = "Tate",
                EmailAddress = "tatey@gmail.com",
                Language = Language.English,
                PhoneNumber = "342 234 954",
                JobId = new Guid("5dcd897b-4b68-4346-8f96-943b27d5a8ac"),
                Attachment = Array.Empty<byte>()
            });
            
            appContext.Add(new Candidate
            {
                Id = new Guid("b0616075-a308-4720-b8d8-c2b396035331"),
                FirstName = "Candidate",
                LastName = "To Be Deleted",
                EmailAddress = "DeleteTest@gmail.com",
                Language = Language.German,
                PhoneNumber = "654 983 094",
                JobId = new Guid("5dcd897b-4b68-4346-8f96-943b27d5a8ac"),
                Attachment = Array.Empty<byte>()
            });
            
            appContext.SaveChanges();
        });


    }
}