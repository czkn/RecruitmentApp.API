namespace RecruitmentApp.API.Employees;

public static class EmployeeMapper
{
    public static Employee CreateEmployeeDtoToEmployee(this CreateEmployeeDTO createEmployeeDto)
    {
        return new Employee
        {
            FirstName = createEmployeeDto.FirstName,
            LastName = createEmployeeDto.LastName,
            Language = createEmployeeDto.Language,
            Localization = createEmployeeDto.Localization,
            WorkModel = createEmployeeDto.WorkModel,
            EmailAddress = createEmployeeDto.EmailAddress,
            PhoneNumber = createEmployeeDto.PhoneNumber,
            WorkingTimeInHours = createEmployeeDto.WorkingTimeInHours,
            WorkingHours = createEmployeeDto.WorkingHours
        };
    }
}