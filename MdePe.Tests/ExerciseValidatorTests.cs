using FluentValidation.TestHelper;
using MdePe.Domain.Models;
using MdePe.Domain.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MdePe.Tests
{
    public class ExerciseValidatorTests
    {
        

        [Fact]
        public void Validate_NameIsEmpty_ShouldHaveError()
        {
            // Arrange
            var model = new Exercise { Name = "" };
            var validator = new ExerciseValidator();
            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(e => e.Name).WithErrorMessage("Please enter a name!");
        }

        [Fact]
        public void Validate_DescriptionIsEmpty_ShouldHaveError()
        {
            // Arrange
            var model = new Exercise { Description = "" };
            var validator = new ExerciseValidator();
            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(e => e.Description).WithErrorMessage("Please enter a description!");
        }

        [Fact]
        public void Validate_RepsIsZeroOrLess_ShouldHaveError()
        {
            // Arrange
            var model = new Exercise { Reps = 0 };
            var validator = new ExerciseValidator();
            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(e => e.Reps).WithErrorMessage("Please enter a valid amount of reps!");
        }

        [Fact]
        public void Validate_WeightIsNegative_ShouldHaveError()
        {
            // Arrange
            var model = new Exercise { Weight = -1 };
            var validator = new ExerciseValidator();
            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(e => e.Weight).WithErrorMessage("'Weight' must be greater than or equal to '0'.");
        }

        [Fact]
        public void Validate_SetsIsZeroOrLess_ShouldHaveError()
        {
            // Arrange
            var model = new Exercise { Sets = 0 };
            var validator = new ExerciseValidator();
            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(e => e.Sets).WithErrorMessage("Sets must be at least 1!");
        }

        [Fact]
        public void Validate_MuscleGroupNameIsNull_ShouldHaveError()
        {
            // Arrange
            var model = new Exercise { MuscleGroupName = null };
            var validator = new ExerciseValidator();
            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(e => e.MuscleGroupName).WithErrorMessage("Please select a muscle group!");
        }

        [Fact]
        public void Validate_WithValidInput_ShouldNotHaveError()
        {
            // Arrange
            var model = new Exercise
            {
                Name = "Push Up",
                Description = "A basic push up exercise",
                Reps = 10,
                Weight = 10,
                Sets = 3,
                MuscleGroupName = new MuscleGroup { Name = "Chest", Id = 1 }
            };
            var validator = new ExerciseValidator();
            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(e => e.Name);
            result.ShouldNotHaveValidationErrorFor(e => e.Description);
            result.ShouldNotHaveValidationErrorFor(e => e.Reps);
            result.ShouldNotHaveValidationErrorFor(e => e.Weight);
            result.ShouldNotHaveValidationErrorFor(e => e.Sets);
            result.ShouldNotHaveValidationErrorFor(e => e.MuscleGroupName);
        }

    }
}
