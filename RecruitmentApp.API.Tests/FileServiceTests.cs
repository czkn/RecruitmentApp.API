using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using RecruitmentApp.API.Candidates;
using Xunit;

namespace RecruitmentApp.API.Tests;

public class FileServiceTests
{
    [Fact]
    public async Task GetFileShouldReturnFileContentResult()
    {
        var candidate = new Candidate()
        {
            Id = new Guid("876d1be3-3430-48e7-8997-a479a11cff8e"),
            FirstName = "George",
            LastName = "Bush",
            EmailAddress = "sdfgasd@gdg.com",
            PhoneNumber = "908 347 832",
            Attachment = new byte[] { Byte.MaxValue }
        };

        var fileService = new FileService();

        var result = fileService.GetFile(candidate);

        Assert.Contains("FileContentResult", result.ToString());
    }
    
    [Fact]
    public async Task GetFileShouldReturnCvWithProperName()
    {
        var candidate = new Candidate()
        {
            Id = new Guid("876d1be3-3430-48e7-8997-a479a11cff8e"),
            FirstName = "George",
            LastName = "Bush",
            EmailAddress = "sdfgasd@gdg.com",
            PhoneNumber = "908 347 832",
            Attachment = new byte[] { Byte.MaxValue }
        };

        var fileService = new FileService();

        var result = fileService.GetFile(candidate);

        Assert.Contains("CV George Bush.pdf", result.FileDownloadName);
    }
    
    [Fact]
    public async Task SaveFileShouldReturnCreatedStatusCode()
    {
        await using var stream = File.OpenRead("placeholder.pdf");
        var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
        {
            Headers = new HeaderDictionary(),
            ContentType = "application/pdf"
        };

        var fileService = new FileService();

        var result = await fileService.SaveFile(file, new Candidate());

        Assert.Contains("Created", result.ToString());
    }
    
    [Fact]
    public async Task SaveFileShouldReturnBadRequest()
    {
        IFormFile? file = null;

        var fileService = new FileService();

        var result = await fileService.SaveFile(file, new Candidate());

        Assert.Contains("BadRequest", result.ToString());
    }
}