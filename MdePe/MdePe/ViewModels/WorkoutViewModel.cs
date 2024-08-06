using FreshMvvm;
using MdePe.Domain.Models;
using MdePe.Domain.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MdePe.ViewModels
{
    class WorkoutViewModel : FreshBasePageModel
    {
        protected readonly IWorkoutService _workoutService;
        protected readonly ISecureTokenService _secureTokenService;

        private ObservableCollection<Workout> workouts;

        public ObservableCollection<Workout> Workouts
        {
            get { return workouts; }
            set { workouts = value; RaisePropertyChanged(nameof(Workouts)); }
        }

        private Workout selectedWorkout;

        public Workout SelectedWorkout
        {
            get => selectedWorkout;
            set
            {
                selectedWorkout = value;
                GoToWorkoutDetailPage.Execute(null);
            }
        }
        private bool isBusy;

        public bool IsBusy
        {
            get { return isBusy; }
            set { isBusy = value; RaisePropertyChanged(nameof(IsBusy)); }
        }
        private bool isTrainer;

        public bool IsTrainer
        {
            get { return isTrainer; }
            set { isTrainer = value; RaisePropertyChanged(nameof(IsTrainer)); }
        }

        public WorkoutViewModel(IWorkoutService workoutService, ISecureTokenService secureTokenService)
        {
            _workoutService = workoutService;
            _secureTokenService = secureTokenService;
        }
        public override void Init(object initData)
        {
            base.Init(initData);
            RefreshData.Execute(null);
        }
        public override void ReverseInit(object returnedData)
        {
            base.ReverseInit(returnedData);
            RefreshData.Execute(null);
        }
        public ICommand RefreshData
        {
            get
            {
                return new Command(async () =>
                {
                    IsBusy = true;
                    var fetchedWorkouts = await _workoutService.GetWorkouts();
                    Workouts = new ObservableCollection<Workout>(fetchedWorkouts.OrderBy(w => w.Name).ToList());
                    IsTrainer = await CheckIfTrainerAsync();
                    IsBusy = false;
                });
            }
        }

        public ICommand GoToExercisePage
        {
            get
            {
                return new Command(() =>
                {
                    CoreMethods.PushPageModel<ExerciseViewModel>();
                });

            }
        }
        public ICommand GoToWorkoutPage
        {
            get
            {
                return new Command(() =>
                {
                    CoreMethods.PushPageModel<WorkoutViewModel>();
                });

            }
        }
        public ICommand GoToSettingsPage
        {
            get
            {
                return new Command(() =>
                {
                    CoreMethods.PushPageModel<SettingsViewModel>();
                });

            }
        }
        public ICommand GoToWorkoutDetailPage
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
        public ICommand CreateWorkout
        {
            get
            {
                return new Command(async () =>
                {
                    await CoreMethods.PushPageModel<WorkoutCreateViewModel>();
                });
            }
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
