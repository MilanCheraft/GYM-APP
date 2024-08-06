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
    public class RegisterValidatorTests
    {
        [Fact]
        public void Validate_FirstNameIsEmpty_ShouldHaveError()
        {
            // Arrange
            var model = new User { FirstName = "" };
            var validator = new RegisterValidator();

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(u => u.FirstName).WithErrorMessage("Please enter a first name!");
        }

        [Fact]
        public void Validate_LastNameIsEmpty_ShouldHaveError()
        {
            // Arrange
            var model = new User { LastName = "" };
            var validator = new RegisterValidator();

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(u => u.LastName).WithErrorMessage("Please enter a last name!");
        }

        [Fact]
        public void Validate_EmailIsEmpty_ShouldHaveError()
        {
            // Arrange
            var model = new User { Email = "" };
            var validator = new RegisterValidator();

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(u => u.Email).WithErrorMessage("Please enter a valid email addresss!");
        }

        [Fact]
        public void Validate_PasswordIsEmpty_ShouldHaveError()
        {
            // Arrange
            var model = new User { Password = "" };
            var validator = new RegisterValidator();

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(u => u.Password).WithErrorMessage("Please enter a password!");
        }

        [Fact]
        public void Validate_PasswordIsTooShort_ShouldHaveError()
        {
            // Arrange
            var model = new User { Password = "abc" };
            var validator = new RegisterValidator();

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(u => u.Password).WithErrorMessage("Password must be at least 4 characters");
        }

        [Fact]
        public void Validate_WeightIsEmpty_ShouldHaveError()
        {
            // Arrange
            var model = new User { Weight = 0 };
            var validator = new RegisterValidator();

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(u => u.Weight).WithErrorMessage("Please enter your weight!");
        }

        [Fact]
        public void Validate_DateOfBirthIsInTheFuture_ShouldHaveError()
        {
            // Arrange
            var model = new User { DateOfBirth = DateTime.Now.AddDays(1) };
            var validator = new RegisterValidator();

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(u => u.DateOfBirth).WithErrorMessage("Date of birth must be in the past!");
        }
    }
}

