using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DotNetEnv;
using Microsoft.IdentityModel.Tokens;

namespace RecruitmentApp.API.Users;

public class JwtService : IJwtService
{
    public string GenerateJwtToken(string userEmail, IList<string> roles)
    {
        Env.Load();
        
        var secretKeyFromEnv = Environment.GetEnvironmentVariable("SECRET_KEY");
        var issuerFromEnv = Environment.GetEnvironmentVariable("ISSUER");
        var audienceFromEnv = Environment.GetEnvironmentVariable("AUDIENCE");
        var accessTokenExpiration = Environment.GetEnvironmentVariable("ACCESS_TOKEN_EXPIRATION");
        
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKeyFromEnv ?? throw new InvalidOperationException()));
        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, userEmail)
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var tokenOptions = new JwtSecurityToken(
            issuer: issuerFromEnv,
            audience: audienceFromEnv,
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(accessTokenExpiration)),
            signingCredentials: signingCredentials
        );
        
        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        return tokenString;
    }
}