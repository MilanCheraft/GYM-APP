using FluentValidation;
using MdePe.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MdePe.Domain.Validators
{
    public class RegisterValidator : AbstractValidator<User>
    {
        public RegisterValidator()
        {
            RuleFor(u => u.FirstName)
                .NotEmpty()
                .WithMessage("Please enter a first name!");
            RuleFor(u => u.LastName)
               .NotEmpty()
               .WithMessage("Please enter a last name!");
            RuleFor(u => u.Email)
                .EmailAddress()
                .NotEmpty()
                .WithMessage("Please enter a valid email addresss!");
            RuleFor(u => u.Password)
                .NotEmpty()
                .WithMessage("Please enter a password!")
                .MinimumLength(4)
                .WithMessage("Password must be at least 4 characters");
            RuleFor(u => u.Weight)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("Please enter your weight!");
            RuleFor(u => u.DateOfBirth)
                .LessThan(d => DateTime.Now)
                .WithMessage("Date of birth must be in the past!");
            
        }
    }
}
