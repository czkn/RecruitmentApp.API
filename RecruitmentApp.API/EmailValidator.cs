namespace RecruitmentApp.API;

public static class EmailValidator
{
    public static bool IsValidEmail(this string email)
    {
        var trimmedEmail = email.Trim();

        if (trimmedEmail.EndsWith(".")) {
            return false;
        }

        if (!trimmedEmail.EndsWith(".com") && !trimmedEmail.EndsWith(".pl"))
            return false;
        
        var addr = new System.Net.Mail.MailAddress(email);
            
        return addr.Address == trimmedEmail;

    }
}