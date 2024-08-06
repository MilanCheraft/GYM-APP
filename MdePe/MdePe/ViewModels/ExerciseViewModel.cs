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
using Xamarin.Forms;

namespace MdePe.ViewModels
{
    public class ExerciseViewModel : FreshBasePageModel
    {
        protected readonly IExerciseService _exerciseService;
        protected readonly ISecureTokenService _secureTokenService;

        public ExerciseViewModel(IExerciseService exerciseService, ISecureTokenService secureTokenService)
        {
            _exerciseService = exerciseService; 
            _secureTokenService = secureTokenService;
        }

        private ObservableCollection<Exercise> exercises;
        public ObservableCollection<Exercise> Exercises
        {
            get { return exercises; }
            set { exercises = value; RaisePropertyChanged(nameof(Exercises)); }
        }
        private bool isTrainer;

        public bool IsTrainer
        {
            get { return isTrainer; }
            set { isTrainer = value; RaisePropertyChanged(nameof(IsTrainer)); }
        }


        private Exercise selectedExercise;

        public Exercise SelectedExercise
        {
            get => selectedExercise;
            set
            {
                selectedExercise = value;
                GoToDetailPage.Execute(null);
            }
        }
        private bool isBusy;

        public bool IsBusy
        {
            get { return isBusy; }
            set { isBusy = value; RaisePropertyChanged(nameof(IsBusy)); }
        }

        public override async void Init(object initData)
        {
            base.Init(initData);
            RefreshData.Execute(null);
            IsTrainer = await CheckIfTrainerAsync();
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
                    var fetchedExercises = await _exerciseService.GetExercises();
                    Exercises = new ObservableCollection<Exercise>(fetchedExercises.OrderBy(e => e.Name).ToList());
                    IsBusy = false;
                });
            }
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
        public ICommand AddExerciseCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await CoreMethods.PushPageModel<ExerciseCreateViewModel>();
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
                return new Command(() =>
                {
                    CoreMethods.PushPageModel<SettingsViewModel>();
                });

            }
        }
        public ICommand GoToDetailPage
        {
            get
            {
                return new Command(async () =>
                {
                    if (SelectedExercise != null)
                        await CoreMethods.PushPageModel<ExerciseDetailViewModel>(SelectedExercise.Id, false, true);
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
