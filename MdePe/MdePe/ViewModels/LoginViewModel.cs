using FreshMvvm;
using MdePe.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MdePe.ViewModels
{
    class LoginViewModel : FreshBasePageModel
    {
        protected IUserService _userService;

        public LoginViewModel(IUserService userService)
        {
            _userService = userService;
        }

        private string email;

        public string Email
        {
            get { return email; }
            set { email = value; RaisePropertyChanged(nameof(Email)); }
        }
        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; RaisePropertyChanged(nameof(Password)); }
        }
        private bool isBusy;

        public bool IsBusy
        {
            get { return isBusy; }
            set { isBusy = value; RaisePropertyChanged(nameof(IsBusy)); }
        }
        private bool canExecute;

        public bool CanExecute
        {
            get { return canExecute; }
            set { canExecute = value; RaisePropertyChanged(nameof(CanExecute)); }
        }

        public override void Init(object initData)
        {
            base.Init(initData);
            CanExecute = true;
        }
        public ICommand LoginCommand
        {
            get
            {
                return new Command(async () =>
                {
                    CanExecute = false;
                    IsBusy = true;
                    if (await ValidateInputAsync(email, password))
                    {
                        await CoreMethods.PushPageModel<WorkoutViewModel>();
                        IsBusy = false;
                        CanExecute = true;
                    }
                    else
                    {
                        await CoreMethods.DisplayAlert("Login", "Wrong credentials!", "OK");
                    }
                    CanExecute = true;
                    IsBusy = false;
                });

            }
        }
        private async Task<bool> ValidateInputAsync(string email, string password)
        {
            if (await _userService.Login(email, password))
            {
                return true;
            }
            return false;
        }


    }
}
