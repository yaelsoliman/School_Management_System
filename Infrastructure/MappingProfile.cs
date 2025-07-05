using Application.DTOs.Assignments;
using Application.DTOs.AssignmentSubmissions;
using Application.DTOs.Courses;
using Application.DTOs.Enrollments;
using Application.DTOs.GradeAssignmentSubmissions;
using Application.DTOs.Users;
using AutoMapper;
using Common.Request.Assignments;
using Common.Request.AssignmentSubmissions;
using Common.Request.Courses;
using Common.Request.Enrollments;
using Common.Request.GradeAssignmentSubmissions;
using Domain.Entities;
using Infrastructure.IdentityModels;

namespace Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AddCourseRequest, Course>();
            CreateMap<Course, CourseDTO>();
            CreateMap<UpdateCourseRequest, Course>();

            CreateMap<AddEnrollmentRequest, Enrollment>();
            CreateMap<Enrollment, EnrollmentDTO>();

            CreateMap<AddAssignmentRequest, Assignment>();
            CreateMap<Assignment, AssignmentDTO>();

            CreateMap<AddAssignmentSubmissionRequest,AssignmentSubmission>();
            CreateMap<AssignmentSubmission, AssignmentSubmissionDTO>();



            CreateMap<AddGradeAssignmentSubmissionRequest, Grade>();
            CreateMap<Grade, GradeAssignmentSubmissionsDTO>();

            CreateMap<User, UserDTO>();

        }

    }
}
