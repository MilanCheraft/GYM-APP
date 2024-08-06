using FreshMvvm;
using MdePe.Domain.Models;
using MdePe.Domain.Services;
using MdePe.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MdePe.ViewModels
{
    public class UpdateWorkoutViewModel : FreshBasePageModel
    {
        protected readonly IExerciseService _exerciseService;
        protected readonly IWorkoutService _workoutService;
        protected readonly IMuscleGroupService _muscleGroupService;

        public UpdateWorkoutViewModel(IExerciseService exerciseService, IWorkoutService workoutService, IMuscleGroupService muscleGroupService)
        {
            _exerciseService = exerciseService;
            _workoutService = workoutService;
            _muscleGroupService = muscleGroupService;
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
        private int muscleGroupIndex;

        public int MuscleGroupIndex
        {
            get { return muscleGroupIndex; }
            set { muscleGroupIndex = value; 
                RaisePropertyChanged(nameof(MuscleGroupIndex)); }
        }


        public override async void Init(object initData)
        {
            IsBusy = true;
            base.Init(initData);
            Id = (int)initData;
            await LoadData();
            IsBusy = false;
            
        }
        private async Task LoadData()
        {
            MuscleGroups = new ObservableCollection<MuscleGroup>(await _muscleGroupService.GetMuscleGroups());
            SelectedExercises = await GetSelectableExercises();
            Name = Workout.Name;
            Description = Workout.Description;
            Duration = Workout.Duration;
            SelectedMuscleGroup = Workout.MuscleGroup;
            for(int i = 0; i < MuscleGroups.Count; i++)
            {
                if (MuscleGroups[i].Id == SelectedMuscleGroup.Id)
                {
                    MuscleGroupIndex = i;
                }
            }
        }
        private async Task<ObservableCollection<SelectableItem>> GetSelectableExercises()
        {
            var exercises = await _exerciseService.GetExercises();
            var selectableExercises = new List<SelectableItem>();
            Workout = await _workoutService.GetWorkoutById(Id);
            foreach (var exercise in exercises)
            {
                selectableExercises.Add(new SelectableItem
                {   
                    Exercise = exercise,
                    IsSelected = false
                });
            }
            var exercisesFromWorkout = Workout.Exercises;
            foreach (var exercise in exercisesFromWorkout)
            {
                selectableExercises.FirstOrDefault(e => e.Exercise.Id == exercise.Id).IsSelected = true;
            }
            return new ObservableCollection<SelectableItem>(selectableExercises);
        }
        public ICommand UpdateCommand
        {
            get
            {
                return new Command(async () =>
                {
                    IsBusy = true;
                    var updatedExercises = SelectedExercises.Where(e => e.IsSelected.Equals(true)).Select(e => e.Exercise);
                    Workout.Name = Name;
                    Workout.Description = Description;
                    Workout.Duration = Workout.Duration;
                    Workout.Exercises =  updatedExercises.ToList();
                    Workout.MuscleGroup = selectedMuscleGroup;

                    var result = await _workoutService.Update(Workout);
                    if (result)
                    {
                        await CoreMethods.PopPageModel();
                        await CoreMethods.PopPageModel();
                        await CoreMethods.PushPageModel<WorkoutDetailsViewModel>(Id, false, true);
                    }
                    else
                    {
                        await CoreMethods.DisplayAlert("Failed", "Failed to update the workout", "OK");

                    }
                    IsBusy = false;
                });
            }
        }
       
    }
}
