﻿using FluentValidation;
using MentorCore.DTO.Courses;

namespace MentorCore.DTO.Validators.Courses
{
    public class CourseModelValidator : AbstractValidator<CourseModel>
    {
        public CourseModelValidator()
        {
            RuleFor(m => m.Id).NotEmpty().WithMessage("Id can't be null or empty");

            RuleFor(m => m.Name).NotEmpty().WithMessage("Name can't be null or empty");

            RuleFor(m => m.Description).NotEmpty().WithMessage("Description can't be null or empty");
        }
    }
}