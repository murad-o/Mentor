﻿using Contracts.Courses;
using FluentValidation;

namespace Contracts.Validators.Courses
{
    public class CreateCourseModelValidator : AbstractValidator<CreateCourseModel>
    {
        public CreateCourseModelValidator()
        {
            RuleFor(m => m.Name)
                .NotEmpty().WithMessage("Name can't be null or empty");

            RuleFor(m => m.Description)
                .NotEmpty().WithMessage("Description can't be null or empty");
        }
    }
}