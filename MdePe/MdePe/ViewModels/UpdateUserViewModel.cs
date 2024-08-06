using FreshMvvm;
using MdePe.Domain.Models;
using MdePe.Domain.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MdePe.ViewModels
{
    public class UpdateUserViewModel : FreshBasePageModel
    {
        protected readonly IUserService _userService;

        public UpdateUserViewModel(IUserService userService)
        {
            _userService = userService;
        }
        private bool isBusy;

        public bool IsBusy
        {
            get { return isBusy; }
            set { isBusy = value; RaisePropertyChanged(nameof(IsBusy)); }
        }
        private string id;

        public string Id
        {
            get { return id; }
            set { id = value; RaisePropertyChanged(nameof(Id)); }
        }


        private User currentUser;

        public User CurrentUser
        {
            get { return currentUser; }
            set { currentUser = value; RaisePropertyChanged(nameof(CurrentUser)); }
        }
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
        private string displayName;

        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; RaisePropertyChanged(nameof(DisplayName)); }
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

        public override async void Init(object initData)
        {
            IsBusy = true;
            base.Init(initData);
            await LoadDataAsync();


        }
        private async Task LoadDataAsync()
        {
            if (Application.Current.Properties.ContainsKey("Username"))
            {
                var currentUserEmail = Application.Current.Properties.FirstOrDefault(p => p.Key == "Username").Value.ToString();
                var user = await _userService.GetUserByEmail(currentUserEmail);

                CurrentUser = user;
                FirstName = user.FirstName;
                LastName = user.LastName;
                DisplayName = user.DisplayName;
                Weight = user.Weight;
                Length = user.Length;
            }
            IsBusy = false;
        }
        public ICommand SaveUser
        {
            get
            {
                return new Command(async () =>
                {
                    var userUpdate = new User
                    {
                        FirstName = FirstName,
                        LastName = LastName,
                        Weight = Weight,
                        Length = Length,
                        DisplayName = DisplayName,
                        Id = currentUser.Id,
                        Email = currentUser.Email,
                    };
                    if (await _userService.Update(userUpdate))
                    {
                        await CoreMethods.PopPageModel();
                        await CoreMethods.PopPageModel();
                        await CoreMethods.PushPageModel<SettingsViewModel>();
                    }
                    else
                    {
                        await CoreMethods.DisplayAlert("Update info", "Failed to update the user information!", "OK");
                    }
                    
                });
            }
        }
    }
}
