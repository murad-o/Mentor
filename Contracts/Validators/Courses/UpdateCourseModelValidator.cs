﻿using Contracts.Courses;
using FluentValidation;

namespace Contracts.Validators.Courses
{
    public class UpdateCourseModelValidator : AbstractValidator<UpdateCourseModel>
    {
        public UpdateCourseModelValidator()
        {
            RuleFor(m => m.Name)
                .NotEmpty().WithMessage("Name can't be null or empty");

            RuleFor(m => m.Description)
                .NotEmpty().WithMessage("Description can't be null or empty");
        }
    }
}