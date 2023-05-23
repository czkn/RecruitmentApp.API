using System.Text.RegularExpressions;

namespace RecruitmentApp.API;

public static class PhoneNumberValidator
{
    public static bool IsValidPhoneNumber(this string number)
    {
        return Regex.Match(number, @"^[(]?[0-9]{3}[)]?[\s\.]?[0-9]{3}[-\s\.]?[0-9]{3,6}$").Success;
    }
}