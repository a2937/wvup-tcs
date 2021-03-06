﻿using System;
using System.Linq;
using System.Threading.Tasks;
using tcs_service.Helpers;
using tcs_service.Models;
using tcs_service.Models.DTO;
using tcs_service.Repos.Interfaces;
using tcs_service.Services.Interfaces;

namespace tcs_service.Services
{
    public class MockBannerService : IBannerService
    {
        private readonly IPersonRepo personRepo;
        private readonly ISemesterRepo semesterRepo;
        private readonly IClassRepo classRepo;

        public MockBannerService(IPersonRepo personRepo, ISemesterRepo semesterRepo, IClassRepo classRepo)
        {
            this.personRepo = personRepo;
            this.semesterRepo = semesterRepo;
            this.classRepo = classRepo;
        }

        public async Task<BannerPersonInfo> GetBannerInfo(string identifier)
        {
            var person = await personRepo.Find(x => x.Email == identifier || x.Id.ToString() == identifier);
            if (person is Person)
            {
                var currentSemester = semesterRepo.GetAll().Last();
                var rand = new Random(person.Id);
                var takeCount = Math.Ceiling(rand.NextDouble() * 5) + 1;
                var randomCourses = classRepo.GetAll().ToList().Take((int)takeCount);
                if (person.PersonType == PersonType.Student)
                {
                    return new BannerPersonInfo()
                    {
                        EmailAddress = person.Email,
                        FirstName = person.FirstName,
                        LastName = person.LastName,
                        WVUPID = person.Id,
                        Teacher = false,
                        TermCode = currentSemester.Code,
                        Courses = randomCourses.Select(x => new BannerClass()
                        {
                            CourseName = x.Name,
                            CRN = x.CRN,
                            ShortName = x.ShortName,
                            Department = new BannerDepartment()
                            {
                                Code = x.Department.Code,
                                Name = x.Department.Name
                            }
                        })
                    };
                }
                else
                {
                    return new BannerPersonInfo()
                    {
                        EmailAddress = person.Email,
                        FirstName = person.FirstName,
                        LastName = person.LastName,
                        WVUPID = person.Id,
                        Teacher = true,
                        TermCode = currentSemester.Code,
                    };
                }
            }
            throw new TCSException($"Person was not found with: {identifier}");
        }


        public async Task<ClassWithGradeDTO> GetStudentGrade(int studentId, int crn, int termCode)
        {
            var grades = Enum.GetValues(typeof(Grade));

            var course = await classRepo.Find(x => x.CRN == crn);

            return new ClassWithGradeDTO()
            {
                CRN = course.CRN,
                CourseName = course.Name,
                DepartmentName = course.Department.Name,
                FinalGrade = (Grade)new Random().Next(grades.Length),
            };
        }
    }
}
