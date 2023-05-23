using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using RecruitmentApp.API.Candidates;
using Xunit;

namespace RecruitmentApp.API.Tests;

public class CandidateTests : IClassFixture<AppFactory<Program>>
{
    private readonly HttpClient _client;

    public CandidateTests(AppFactory<Program> appFactory)
        => _client = appFactory.CreateClient();

    [Fact]
    public async Task GetCandidatesShouldReturnOkStatusCode()
    {
        var getRequest = new HttpRequestMessage(HttpMethod.Get, "/api/Candidate");
        var response = await _client.GetAsync(getRequest.RequestUri);
        
        var responseStatusCode = response.StatusCode.ToString();
        
        Assert.Contains("OK", responseStatusCode);
    }
    
    [Fact]
    public async Task GetCandidatesShouldReturnAllCandidates()
    {
        var getRequest = new HttpRequestMessage(HttpMethod.Get, "/api/Candidate");
        var response = await _client.GetAsync(getRequest.RequestUri);

        var responseContent = await response.Content.ReadAsStringAsync();
        
        Assert.Contains("Jack", responseContent);
        Assert.Contains("Newton", responseContent);
        Assert.Contains("903 894 983", responseContent);
        Assert.Contains("Leon", responseContent);
        Assert.Contains("Tate", responseContent);
        Assert.Contains("342 234 954", responseContent);
    }
    
    [Fact]
    public async Task GetCandidateShouldReturnOkStatusCode()
    {
        var getRequest = new HttpRequestMessage(HttpMethod.Get, "/api/Candidate/157439c5-d908-47ff-a648-f9ac059f2ea2");
        var response = await _client.GetAsync(getRequest.RequestUri);
        
        var responseStatusCode = response.StatusCode.ToString();
        Assert.Contains("OK", responseStatusCode);
    }
    
    [Fact]
    public async Task GetCandidateShouldReturnASpecificCandidate()
    {
        var getRequest = new HttpRequestMessage(HttpMethod.Get, "/api/Candidate/157439c5-d908-47ff-a648-f9ac059f2ea2");
        var response = await _client.GetAsync(getRequest.RequestUri);

        var responseContent = await response.Content.ReadAsStringAsync();
        
        Assert.Contains("Jack", responseContent);
        Assert.Contains("Newton", responseContent);
        Assert.Contains("jnewton@gmail.com", responseContent);
        Assert.Contains("903 894 983", responseContent);
        Assert.Contains("5dcd897b-4b68-4346-8f96-943b27d5a8ac", responseContent);
    }
    
    [Fact]
    public async Task DeleteCandidate_ShouldReturnNoContent()
    {
        HttpResponseMessage response = await _client.DeleteAsync("/api/Candidate/b0616075-a308-4720-b8d8-c2b396035331");
        
        var responseStatusCodeString = response.StatusCode.ToString();
        Assert.Contains("NoContent", responseStatusCodeString);
    }
    
    [Fact]
    public async Task GetCandidate_ShouldReturnNotFound()
    {
        await _client.DeleteAsync("/api/Candidate/b0616075-a308-4720-b8d8-c2b396035331");
        
        var getRequest = new HttpRequestMessage(HttpMethod.Get, "/api/Candidate/b0616075-a308-4720-b8d8-c2b396035331");
        HttpResponseMessage response = await _client.GetAsync(getRequest.RequestUri);
        
        var responseStatusCodeString = response.StatusCode.ToString();
        Assert.Contains("NotFound", responseStatusCodeString);
    }

}