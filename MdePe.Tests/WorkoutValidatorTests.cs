using FluentValidation.TestHelper;
using MdePe.Domain.Models;
using MdePe.Domain.Validators;
using System.Collections.Generic;
using Xunit;

namespace MdePe.Tests
{
    public class WorkoutValidatorTests
    {
        [Fact]
        public void Validate_NameIsEmpty_ShouldHaveError()
        {
            // Arrange
            var model = new Workout { Name = "" };
            var validator = new WorkoutValidator();

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(w => w.Name);
        }

        [Fact]
        public void Validate_DescriptionIsEmpty_ShouldHaveError()
        {
            // Arrange
            var model = new Workout { Description = "" };
            var validator = new WorkoutValidator();

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(w => w.Description);
        }

        [Fact]
        public void Validate_DurationIsZeroOrLess_ShouldHaveError()
        {
            // Arrange
            var model = new Workout { Duration = 0 };
            var validator = new WorkoutValidator();

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(w => w.Duration);
        }

        [Fact]
        public void Validate_MuscleGroupIsEmpty_ShouldHaveError()
        {
            // Arrange
            var model = new Workout { MuscleGroup = null };
            var validator = new WorkoutValidator();

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(w => w.MuscleGroup);
        }

        [Fact]
        public void Validate_ExercisesIsNull_ShouldHaveError()
        {
            // Arrange
            var model = new Workout { Exercises = null };
            var validator = new WorkoutValidator();

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(w => w.Exercises);
        }

        [Fact]
        public void Validate_ExercisesCountIsZero_ShouldHaveError()
        {
            // Arrange
            var model = new Workout { Exercises = new List<Exercise>() };
            var validator = new WorkoutValidator();

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(w => w.Exercises);
        }
    }
}
