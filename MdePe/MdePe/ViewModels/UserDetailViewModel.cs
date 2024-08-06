using FreshMvvm;
using MdePe.Domain.Models;
using MdePe.Domain.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MdePe.ViewModels
{
    public class UserDetailViewModel: FreshBasePageModel
    {
        protected readonly IUserService _userService;
        protected readonly IWorkoutService _workoutService;

        public UserDetailViewModel(IUserService userService, IWorkoutService workoutService)
        {
            _userService = userService;
            _workoutService = workoutService;
        }
        private User currentUser;

        public User CurrentUser
        {
            get { return currentUser; }
            set { currentUser = value; RaisePropertyChanged(nameof(CurrentUser)); }
        }
        private Workout selectedWorkout;

        public Workout SelectedWorkout
        {
            get => selectedWorkout;
            set
            {
                selectedWorkout = value;
                GoToWorkoutDetails.Execute(null);
            }
        }
        private ObservableCollection<Workout> workouts;

        public ObservableCollection<Workout> Workouts
        {
            get { return workouts; }
            set { workouts = value; RaisePropertyChanged(nameof(Workouts)); }
        }

        private string email;

        public string Email
        {
            get { return email; }
            set { email = value; RaisePropertyChanged(nameof(Email)); }
        }
        private bool hasNoWorkouts;

        public bool HasNoWorkouts
        {
            get { return hasNoWorkouts; }
            set { hasNoWorkouts = value; RaisePropertyChanged(nameof(HasNoWorkouts)); }
        }

        public override async void Init(object initData)
        {
            base.Init(initData);
            Email = initData.ToString();
            await LoadDataAsync();
        }
        private async Task LoadDataAsync()
        {
            CurrentUser = await _userService.GetUserByEmail(Email);
            var workouts = await _workoutService.GetWorkoutsByUserIdAsync(CurrentUser.Id);
            if(workouts.Count == 0)
            {
                HasNoWorkouts = true;
            }
            hasNoWorkouts = false;
            Workouts = new ObservableCollection<Workout>(workouts);
        }
        public ICommand GoToWorkoutDetails
        {
            get
            {
                return new Command(async () =>
                {
                    if (SelectedWorkout != null)
                        await CoreMethods.PushPageModel<WorkoutDetailsViewModel>(SelectedWorkout.Id, false, true);
                });
            }
        }
    }
}
