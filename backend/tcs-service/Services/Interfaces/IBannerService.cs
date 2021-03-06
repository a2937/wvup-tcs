﻿using System.Collections.Generic;
using System.Threading.Tasks;
using tcs_service.Models.DTO;

namespace tcs_service.Services.Interfaces
{
    public interface IBannerService
    {
        Task<BannerPersonInfo> GetBannerInfo(string identifier);
        Task<ClassWithGradeDTO> GetStudentGrade(int studentId, int crn, int termCode);
    }
}

public class BannerPersonInfo
{
    public int WVUPID { get; set; }
    public string EmailAddress { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int TermCode { get; set; }
    public bool Teacher { get; set; }
    public IEnumerable<BannerClass> Courses { get; set; }
}


public class BannerClass
{
    public string CourseName { get; set; }
    public string ShortName { get; set; }
    public int CRN { get; set; }
    public BannerDepartment Department { get; set; }
}

public class BannerDepartment
{
    public string Name { get; set; }
    public int Code { get; set; }
}