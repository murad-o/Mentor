﻿using AutoMapper;
using Entities.Models;
using MentorCore.DTO.Courses;

namespace MentorCore.Services.Automapper
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
