using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecruitmentApp.API.Employees;
using Xunit;

namespace RecruitmentApp.API.Tests;

public class EmployeeTests : IClassFixture<AppFactory<Program>>
{
    private readonly HttpClient _client;

    public EmployeeTests(AppFactory<Program> appFactory)
        => _client = appFactory.CreateClient();
    
    [Fact]
    public async Task PostEmployee_ShouldReturnCreatedStatusCode()
    {
        HttpResponseMessage response = 
            await _client.PostAsJsonAsync(new HttpRequestMessage(HttpMethod.Post, "/api/Employee").RequestUri,
                CreateInMemoryEmployeeDto("astoklosa@gmail.com", "387 983 872"));
        
        var responseStatusCode = response.StatusCode.ToString();

        Assert.Contains("Created", responseStatusCode);
    }
    
    [Fact]
    public async Task PostEmployeeWithIncorrectPhoneNumber_ShouldReturnBadRequest()
    {
        HttpResponseMessage response = 
            await _client.PostAsJsonAsync(new HttpRequestMessage(HttpMethod.Post, "/api/Employee").RequestUri,
                CreateInMemoryEmployeeDto("astoklosa@gmail.com", "908 098 987124435"));
        
        var responseStatusCode = response.StatusCode.ToString();

        Assert.Contains("BadRequest", responseStatusCode);
    }
    
    [Fact]
    public async Task PostEmployeeWithIncorrectEmail_ShouldReturnBadRequest()
    {
        HttpResponseMessage response = 
            await _client.PostAsJsonAsync(new HttpRequestMessage(HttpMethod.Post, "/api/Employee").RequestUri,
                CreateInMemoryEmployeeDto("astoklosa@gmail", "908 098 987"));
        
        var responseStatusCode = response.StatusCode.ToString();

        Assert.Contains("BadRequest", responseStatusCode);
    }
    
    private static CreateEmployeeDTO CreateInMemoryEmployeeDto(string email, string phoneNumber)
    {
        var context = GetInMemoryDatabaseContext();

        var inMemoryEmployeeDto = new CreateEmployeeDTO
        {
            FirstName = "Andrzej",
            LastName = "Stoklosa",
            EmailAddress = email,
            Language = Language.French,
            Localization = "Warsaw",
            WorkModel = WorkModel.Remote,
            PhoneNumber = phoneNumber,
            WorkingTimeInHours = 8,
            WorkingHours = "8-16",
        };
        
        context.Database.EnsureDeleted();
        
        return inMemoryEmployeeDto;
    }

    private static RecruitmentAppDbContext GetInMemoryDatabaseContext()
    {
        var options = new DbContextOptionsBuilder<RecruitmentAppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        
        var databaseContext = new RecruitmentAppDbContext(options);

        return databaseContext;
    }
    
}
    
