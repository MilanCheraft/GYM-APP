using FreshMvvm;
using MdePe.Domain.Models;
using MdePe.Domain.Services;
using MdePe.Domain.Validators;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MdePe.ViewModels
{
    public class RegisterViewModel : FreshBasePageModel
    {
        protected IUserService _userService;

        public RegisterViewModel(IUserService userService)
        {
            _userService = userService;
        }


        #region ErrorStrings


        private string repeatPasswordError;

        public string RepeatPasswordError
        {
            get { return repeatPasswordError; }
            set { repeatPasswordError = value; RaisePropertyChanged(nameof(RepeatPassword)); }
        }
        private string emailError;

        public string EmailError
        {
            get { return emailError; }
            set { emailError = value; RaisePropertyChanged(nameof(EmailError)); }
        }
        private string weightError;

        public string WeightError
        {
            get { return weightError; }
            set { weightError = value; RaisePropertyChanged(nameof(WeightError)); }
        }

        private string dobError;

        public string DobError
        {
            get { return dobError; }
            set { dobError = value; RaisePropertyChanged(nameof(DobError)); }
        }
        private string passwordError;

        public string PasswordError
        {
            get { return passwordError; }
            set { passwordError = value; RaisePropertyChanged(nameof(PasswordError)); }
        }
        private string firstNameError;

        public string FirstNameError
        {
            get { return firstNameError; }
            set { firstNameError = value; RaisePropertyChanged(nameof(firstNameError)); }
        }
        private string lastNameError;

        public string LastNameError
        {
            get { return lastNameError; }
            set { lastNameError = value; RaisePropertyChanged(nameof(LastNameError)); }
        }

        #endregion

        private string firstName;

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; RaisePropertyChanged(nameof(FirstName)); }
        }
        private string lastName;

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; RaisePropertyChanged(nameof(LastName)); }
        }
        private string email;

        public string Email
        {
            get { return email; }
            set { email = value; RaisePropertyChanged(nameof(Email)); }
        }
        private string passsword;

        public string Password
        {
            get { return passsword; }
            set { passsword = value; RaisePropertyChanged(nameof(Password)); }
        }
        private string repeatPassword;

        public string RepeatPassword
        {
            get { return repeatPassword; }
            set { repeatPassword = value; RaisePropertyChanged(nameof(RepeatPassword)); }
        }
        private double weight;

        public double Weight
        {
            get { return weight; }
            set { weight = value; RaisePropertyChanged(nameof(Weight)); }
        }
        private int length;

        public int Length
        {
            get { return length; }
            set { length = value; RaisePropertyChanged(nameof(Length)); }
        }
        private DateTime dateOfBirth;

        public DateTime DateOfBirth
        {
            get { return dateOfBirth; }
            set { dateOfBirth = value; RaisePropertyChanged(nameof(DateOfBirth)); }
        }
        public override void Init(object initData)
        {
            base.Init(initData);
            DateOfBirth = DateTime.Now;
        }
        private bool isBusy;

        public bool IsBusy
        {
            get { return isBusy; }
            set { isBusy = value; RaisePropertyChanged(nameof(IsBusy)); }
        }

        public ICommand RegisterCommand
        {
            get
            {
                return new Command(async () =>
                {
                    IsBusy = true;
                    if (Password != RepeatPassword)
                    {
                        await CoreMethods.DisplayAlert("Password", "Passwords does not match!", "OK");
                        IsBusy = false;
                    }
                    else
                    {
                        User newUser = new User
                        {
                            DisplayName = $"{FirstName} {LastName}",
                            Email = Email,
                            Weight = Weight,
                            LastName = LastName,
                            FirstName = FirstName,
                            DateOfBirth = DateOfBirth,
                            Length = Length,
                            Password = Password                            
                        };
                        if (await Validate(newUser))
                        {
                            if (await _userService.Register(newUser))
                            {
                            await CoreMethods.DisplayAlert("Registered", "You are registered! Please login to your account!", "OK");
                            await CoreMethods.PushPageModel<LoginViewModel>();              
                                IsBusy = false;
                            }
                            else
                            {
                                await CoreMethods.DisplayAlert("Oops", "Something went wrong, please try again!", "OK");
                                IsBusy = false;
                            }
                        }
                        IsBusy = false;
                    }
                });
            }
        }
        private async Task<bool> Validate(User newUser)
        {
            RegisterValidator validator = new RegisterValidator();
            var result = validator.Validate(newUser);
            foreach (var error in result.Errors)
            {
                if (error.PropertyName == nameof(FirstName))
                {
                    await CoreMethods.DisplayAlert("First name", error.ErrorMessage, "OK");
                    return false;
                }
                if (error.PropertyName == nameof(LastName))
                {
                    await CoreMethods.DisplayAlert("Last name", error.ErrorMessage, "OK");
                    return false;
                }
                if (error.PropertyName == nameof(Email))
                {
                    await CoreMethods.DisplayAlert("Email", error.ErrorMessage, "OK");
                    return false;
                }
                if (error.PropertyName == nameof(Password))
                {
                    await CoreMethods.DisplayAlert("Password", error.ErrorMessage, "OK");
                    return false;
                }
                if (error.PropertyName == nameof(Weight))
                {
                    await CoreMethods.DisplayAlert("Weight", error.ErrorMessage, "OK");
                    return false;
                }
                if (error.PropertyName == nameof(DateOfBirth))
                {
                    await CoreMethods.DisplayAlert("Date of birth", error.ErrorMessage, "OK");
                    return false;
                }


            }
            return result.IsValid;
        }

    }
}
