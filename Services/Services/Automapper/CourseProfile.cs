using AutoMapper;
using Contracts.Courses;
using Domain.Entities;

namespace Services.Services.Automapper
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<Course, CourseModel>();
            CreateMap<CreateCourseModel, Course>();
        }
    }
}
