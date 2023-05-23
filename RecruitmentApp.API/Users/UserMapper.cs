namespace RecruitmentApp.API.Users;

public static class UserMapper
{
    public static User RegisterUserDtoToUser(this RegisterUserDto registerUserDto)
    {
        return new User
        {
            UserName = registerUserDto.Email,
            Email = registerUserDto.Email,
            FirstName = registerUserDto.FirstName,
            LastName = registerUserDto.LastName,
            CompanyName = registerUserDto.CompanyName
        };
    }
    
    public static GetUserDto UserToGetUserDto(this User user)
    {
        return new GetUserDto
        {
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName
        };
    }
}