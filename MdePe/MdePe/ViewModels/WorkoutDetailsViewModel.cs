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
    public class WorkoutDetailsViewModel : FreshBasePageModel
    {
        protected readonly IWorkoutService _workoutService;
        protected readonly ISecureTokenService _secureTokenService;

        public WorkoutDetailsViewModel(IWorkoutService workoutService, ISecureTokenService secureTokenService)
        {
            _workoutService = workoutService;
            _secureTokenService = secureTokenService;
        }

        private ObservableCollection<Exercise> exercises;

        public ObservableCollection<Exercise> Exercises
        {
            get { return exercises; }
            set { exercises = value; RaisePropertyChanged(nameof(Exercises)); }
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
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; RaisePropertyChanged(nameof(Name)); }
        }
        private string description;

        public string Description
        {
            get { return description; }
            set { description = value; RaisePropertyChanged(nameof(Description)); }
        }
        private int duration;

        public int Duration
        {
            get { return duration; }
            set { duration = value; RaisePropertyChanged(nameof(Duration)); }
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


        public override void Init(object initData)
        {
            base.Init(initData);

            Id = (int)initData;

            LoadData.Execute(null);
        }

        public ICommand LoadData
        {
            get
            {
                return new Command(async () =>
                {
                    IsBusy = true;
                    var workout = await _workoutService.GetWorkoutById(Id);
                    Name = workout.Name;
                    Description = workout.Description;
                    Duration = workout.Duration;
                    Exercises = new ObservableCollection<Exercise>(workout.Exercises.ToList());
                    IsTrainer = await CheckIfTrainerAsync();
                    IsBusy = false;
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
                    {
                        await CoreMethods.PushPageModel<ExerciseDetailViewModel>(SelectedExercise.Id, false, true);
                    }
                });
            }
        }
        public ICommand EditWorkoutCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await CoreMethods.PushPageModel<UpdateWorkoutViewModel>(Id, false, true);
                });
            }
        }
        public ICommand DeleteWorkoutCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (await ConfirmDeleteAsync())
                    {
                        try
                        {
                            await _workoutService.DeleteAsync(Id);
                            await CoreMethods.PopPageModel();
                            await CoreMethods.PopPageModel();
                            await CoreMethods.PushPageModel<WorkoutViewModel>();
                        }
                        catch
                        {
                            await CoreMethods.DisplayAlert("Delete failed", "The workout could not be deleted, please try again", "OK");
                        }
                    }
                });
            }
        }
        private async Task<bool> ConfirmDeleteAsync()
        {
            return await CoreMethods.DisplayAlert("Confirm", "Are you sure you want to delete this workout", "Yes", "No");
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
