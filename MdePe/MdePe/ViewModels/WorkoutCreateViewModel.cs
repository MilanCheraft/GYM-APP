using FreshMvvm;
using MdePe.Domain.Models;
using MdePe.Domain.Services;
using MdePe.Domain.Validators;
using MdePe.Infrastructure.Models;
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
    public class WorkoutCreateViewModel : FreshBasePageModel
    {
        protected readonly IExerciseService _exerciseService;
        protected readonly IWorkoutService _workoutService;
        protected readonly IMuscleGroupService _muscleGroupService;
        protected readonly IUserService _userService;

        public WorkoutCreateViewModel(IWorkoutService workoutService, IExerciseService exerciseService, IMuscleGroupService muscleGroupService, IUserService userService)
        {
            _workoutService = workoutService;
            _exerciseService = exerciseService;
            _muscleGroupService = muscleGroupService;
            _userService = userService;
        }

        private ObservableCollection<SelectableItem> selectedExercises;

        public ObservableCollection<SelectableItem> SelectedExercises
        {
            get { return selectedExercises; }
            set { selectedExercises = value; RaisePropertyChanged(nameof(SelectedExercises)); }
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
        private Workout workout;

        public Workout Workout
        {
            get { return workout; }
            set { workout = value; RaisePropertyChanged(nameof(Workout)); }
        }

        public bool IsBusy
        {
            get { return isBusy; }
            set { isBusy = value; RaisePropertyChanged(nameof(IsBusy)); }
        }
        private MuscleGroup selectedMuscleGroup;

        public MuscleGroup SelectedMuscleGroup
        {
            get { return selectedMuscleGroup; }
            set { selectedMuscleGroup = value; RaisePropertyChanged(nameof(SelectedMuscleGroup)); }
        }
        private ObservableCollection<MuscleGroup> muscleGroups;

        public ObservableCollection<MuscleGroup> MuscleGroups
        {
            get { return muscleGroups; }
            set { muscleGroups = value; RaisePropertyChanged(nameof(MuscleGroups)); }
        }


        public override async void Init(object initData)
        {
            IsBusy = true;
            base.Init(initData);
            await LoadData();
            IsBusy = false;

        }
        private async Task LoadData()
        {
            MuscleGroups = new ObservableCollection<MuscleGroup>(await _muscleGroupService.GetMuscleGroups());
            var exercises = await _exerciseService.GetExercises();
            var selectableExercises = new List<SelectableItem>();
            foreach (var exercise in exercises)
            {
                selectableExercises.Add(new SelectableItem
                {
                    Exercise = exercise,
                    IsSelected = false
                });
            }
            SelectedExercises = new ObservableCollection<SelectableItem>(selectableExercises);
        }
        public ICommand CreateCommand
        {
            get
            {
                return new Command(async () =>
                {

                    var userEmail = Application.Current.Properties.FirstOrDefault(p => p.Key == "Username").Value.ToString();
                    var currentUser = await _userService.GetUserByEmail(userEmail);
                    var workout = new Workout
                    {
                        Name = Name,
                        Description = Description,
                        Duration = Duration,
                        MuscleGroup = SelectedMuscleGroup,
                        Exercises = SelectedExercises.Where(e => e.IsSelected.Equals(true)).Select(e => e.Exercise),
                        UserId = currentUser.Id,
                    };
                    if (await Validate(workout))
                    {
                        var result = await _workoutService.CreateAsync(workout);
                        if (result.Succes)
                        {
                            await CoreMethods.PopPageModel(workout);
                            await CoreMethods.PushPageModel<WorkoutDetailsViewModel>(result.Id, false, true);

                        }
                    }
                });
            }
        }
        private async Task<bool> Validate(Workout workout)
        {
            WorkoutValidator validator = new WorkoutValidator();
            var result = validator.Validate(workout);
            if (!result.IsValid)
            {
                await CoreMethods.DisplayAlert("Error", "Please make sure everything is filled in!", "OK");
            }
            return result.IsValid;
        }
    }
}
