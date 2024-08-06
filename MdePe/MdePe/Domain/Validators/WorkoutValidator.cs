using FluentValidation;
using MdePe.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MdePe.Domain.Validators
{
    public class WorkoutValidator : AbstractValidator<Workout>
    {
        public WorkoutValidator()
        {
            RuleFor(w => w.Name).NotEmpty();
            RuleFor(w => w.Description).NotEmpty();
            RuleFor(w => w.Duration).NotEmpty().GreaterThan(0);
            RuleFor(w => w.MuscleGroup).NotEmpty();
            RuleFor(w => w.Exercises)
               .NotNull()
               .WithMessage("Exercises list cannot be null.")
               .Must(exercises => exercises == null || exercises.Any())
               .WithMessage("At least one exercise must be provided.");
        }

    }
}
