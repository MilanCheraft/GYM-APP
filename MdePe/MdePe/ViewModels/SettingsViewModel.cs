using FreshMvvm;
using MdePe.Domain.Models;
using MdePe.Domain.Services;
using MdePe.Domain.Services.Api;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MdePe.ViewModels
{
    public class SettingsViewModel : FreshBasePageModel
    {
        protected readonly IUserService _userService;
        protected readonly ISecureTokenService _secureTokenService;
        protected readonly NotificationService _notificationService;

        public SettingsViewModel(IUserService userService, ISecureTokenService secureTokenService)
        {
            _userService = userService;
            _secureTokenService = secureTokenService;
            _notificationService = new NotificationService();
        }
        private bool isBusy;

        public bool IsBusy
        {
            get { return isBusy; }
            set { isBusy = value; RaisePropertyChanged(nameof(IsBusy)); }
        }


        private User currentUser;

        public User CurrentUser
        {
            get { return currentUser; }
            set { currentUser = value; RaisePropertyChanged(nameof(CurrentUser)); }
        }
        private string dob;

        public string Dob
        {
            get { return dob; }
            set { dob = value; RaisePropertyChanged(nameof(Dob)); }
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
        private bool isTrainer;

        public bool IsTrainer
        {
            get { return isTrainer; }
            set { isTrainer = value; RaisePropertyChanged(nameof(IsTrainer)); }
        }


        public override async void Init(object initData)
        {
            IsBusy = true;
            base.Init(initData);
            if (Application.Current.Properties.ContainsKey("Username"))
            {
                var currentUserEmail = Application.Current.Properties.FirstOrDefault(p => p.Key == "Username").Value.ToString();
                var user = await _userService.GetUserByEmail(currentUserEmail);

                CurrentUser = user;
                Dob = user.DateOfBirth.ToString("d/M/yyyy");
                Weight = user.Weight;
                Length = user.Length;
            }
            IsBusy = false;
            IsTrainer = await CheckIfTrainerAsync();
        }

        public ICommand GoToExercisePage
        {
            get
            {
                return new Command(async () =>
                {
                    await CoreMethods.PushPageModel<ExerciseViewModel>();
                });

            }
        }
        public ICommand AllUsersCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await CoreMethods.PushPageModel<AllUsersViewModel>();
                });
            }
        }
        public ICommand UpdateProfileCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await CoreMethods.PushPageModel<UpdateUserViewModel>();
                });
            }
        }
        public ICommand GoToWorkoutPage
        {
            get
            {
                return new Command(async () =>
                {
                    await CoreMethods.PushPageModel<WorkoutViewModel>();
                });

            }
        }
        public ICommand GoToSettingsPage
        {
            get
            {
                return new Command(async () =>
                {
                    await CoreMethods.PushPageModel<SettingsViewModel>();
                });

            }
        }
        public ICommand LogoutCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (await ConfirmLogout())
                    {
                        IsBusy = true;
                        if (_userService.Logout())
                        {
                            await CoreMethods.PopToRoot(true);
                            _notificationService.GetNoticicationAsync("We Hate to See You Go", "Thanks for visiting! We hope to see you again soon.");

                        }
                        IsBusy = false;
                    }
                });
            }
        }
        private async Task<bool> ConfirmLogout()
        {
            return await CoreMethods.DisplayAlert("Confirm", "Are you sure you want to log out?", "Yes", "No");
        }
        private async Task<bool> CheckIfTrainerAsync()
        {
            if (await _secureTokenService.GetUserRole() == "Trainer")
            {
                return true;
            }
            return false;
        }
    }
}
