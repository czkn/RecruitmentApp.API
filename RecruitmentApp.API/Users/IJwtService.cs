namespace RecruitmentApp.API.Users;

public interface IJwtService
{
    string GenerateJwtToken(string email, IList<string> roles);
}