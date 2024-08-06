using FluentValidation;
using MdePe.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdePe.Domain.Validators
{
    public class ExerciseValidator : AbstractValidator<Exercise>
    {
        public ExerciseValidator()
        {
            RuleFor(e => e.Name)
                .NotEmpty()
                .WithMessage("Please enter a name!");
            RuleFor(e => e.Description)
               .NotEmpty()
               .WithMessage("Please enter a description!");
            RuleFor(e => e.Reps)
                .GreaterThan(0)
                .NotEmpty()
                .WithMessage("Please enter a valid amount of reps!");
            RuleFor(e => e.Weight)
                .GreaterThanOrEqualTo(0)
                .NotEmpty()
                .WithMessage("Weight can't be lower than 0!");
            RuleFor(e => e.Sets)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("Sets must be at least 1!");
            RuleFor(e => e.MuscleGroupName)
                .NotNull()
                .WithMessage("Please select a muscle group!");

        }
    }
}
