using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace RecruitmentApp.API.Users;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly IJwtService _jwtService;
    private readonly RecruitmentAppDbContext _context;

    public UserController(UserManager<User> userManager, IJwtService jwtService, RecruitmentAppDbContext context)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _context = context;
    }
    
    [Authorize(Policy = "AdminPolicy")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetUserDto>>> GetUsers()
    {
        var users = await _context.Users.ToListAsync();

        return users.Select(u => u.UserToGetUserDto()).ToList();
    }
    
    [Authorize(Policy = "AdminPolicy")]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto)
    {
        if (!ModelState.IsValid) 
            return BadRequest("The model state is not valid");
        
        var userExists = await _userManager.FindByEmailAsync(registerUserDto.Email);
        if (userExists != null)
            return StatusCode(409, new { message = "User with same email already exists" });

        var user = registerUserDto.RegisterUserDtoToUser();

        var result = await _userManager.CreateAsync(user, registerUserDto.Password);

        if (!result.Succeeded) 
            return StatusCode(500, new { message = "Failed to create user" });
        
        await _userManager.AddToRoleAsync(user, "HR");

        return Created("HR User created successfully", user);

    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDto loginUserDto)
    {
        if (!ModelState.IsValid) 
            return BadRequest("The model state is not valid");
    
        var user = await _userManager.FindByEmailAsync(loginUserDto.Email);
        
        if (user == null)
            return Unauthorized(new { message = "Invalid email or password" });

        var isValidPassword = await _userManager.CheckPasswordAsync(user, loginUserDto.Password);
        
        if (!isValidPassword)
            return Unauthorized(new { message = "Invalid email or password" });
        
        var token = _jwtService.GenerateJwtToken(user.Email, await _userManager.GetRolesAsync(user));

        await _userManager.ResetAccessFailedCountAsync(user);
        
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddHours(4),
            IsEssential = true,
            SameSite = SameSiteMode.Lax,
            Domain = "localhost",
            Path = "/",
            Secure = false // set this option to true if the app is served over HTTPS
        };
        
        Response.Cookies.Append("token_cookie", token, cookieOptions);

        return Ok(new { message = "Login successful" });
    }
    
    [Authorize(Policy = "AdminOrHrPolicy")]
    [HttpPost]
    [Route("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("token_cookie", new CookieOptions
        {
            HttpOnly = true,
            Path = "/",
            Expires = DateTimeOffset.UtcNow.AddDays(-1)
        });
    
        return Ok("Logged out successfully");
    }
    
    [Authorize(Policy = "AdminOrHrPolicy")]
    [HttpGet]
    [Route("getUserRole")]
    public IActionResult GetRole()
    {
        var identity = (ClaimsIdentity)User.Identity;
        var roleClaim = identity.FindFirst(ClaimTypes.Role);

        if (roleClaim == null)
        {
            return NotFound();
        }

        return Ok(roleClaim.Value);
    }
    
    [Authorize(Policy = "AdminOrHrPolicy")]
    [HttpGet]
    [Route("getUserId")]
    public async Task<IActionResult> GetUserId()
    {
        var identity = (ClaimsIdentity)User.Identity;
        var userEmailClaim = identity.FindFirst(ClaimTypes.Email);
        
        var user = await _userManager.FindByEmailAsync(userEmailClaim.Value);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(user.Id);
    }
    
    [Authorize(Policy = "AdminPolicy")]
    [HttpDelete("{userEmail}")]
    public async Task<IActionResult> DeleteUser(string userEmail)
    {
        var user = await _userManager.FindByEmailAsync(userEmail);
    
        if (user == null)
        {
            return NotFound();
        }
        
        _context.Users.Remove(user);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}