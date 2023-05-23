namespace RecruitmentApp.API.Jobs;

public static class JobMapper
{
    public static Job CreateJobDtoToJob(this CreateJobDTO createJobDto)
    {
        return new Job
        {
            Name = createJobDto.Name,
            Description = createJobDto.Description,
            ProgrammingLanguage = createJobDto.ProgrammingLanguage,
            Language = createJobDto.Language,
            Localization = createJobDto.Localization,
            WorkModel = createJobDto.WorkModel,
            SalaryMin = createJobDto.SalaryMin,
            SalaryMax = createJobDto.SalaryMax,
            WorkingHours = createJobDto.WorkingHours,
            MandatorySkills = createJobDto.MandatorySkills,
            AppreciatedSkills = createJobDto.AppreciatedSkills,
        };
    }

    public static GetJobDTO JobToGetJobDto(this Job job)
    {
        return new GetJobDTO
        {
            Id = job.Id,
            Name = job.Name,
            Description = job.Description,
            ProgrammingLanguage = job.ProgrammingLanguage,
            Language = job.Language,
            Localization = job.Localization,
            WorkModel = job.WorkModel,
            SalaryMin = job.SalaryMin,
            SalaryMax = job.SalaryMax,
            WorkingHours = job.WorkingHours,
            MandatorySkills = job.MandatorySkills,
            AppreciatedSkills = job.AppreciatedSkills,
            CompanyName = job.CompanyName
        };
    }
}