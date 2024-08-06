using FreshMvvm;
using MdePe.Domain.Models;
using MdePe.Domain.Services;
using MdePe.Domain.Validators;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MdePe.ViewModels
{
    public class ExerciseDetailViewModel : FreshBasePageModel
    {
        protected readonly IExerciseService _exerciseService;
        protected readonly IMuscleGroupService _muscleGroupService;
        protected readonly ISecureTokenService _secureTokenService;

        public ExerciseDetailViewModel(IExerciseService exerciseService, IMuscleGroupService muscleGroupService, ISecureTokenService secureTokenService)
        {
            _exerciseService = exerciseService;
            _muscleGroupService = muscleGroupService;
            _secureTokenService = secureTokenService;
        }

        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        private int reps;

        public int Reps
        {
            get { return reps; }
            set { reps = value; RaisePropertyChanged(nameof(Reps)); }
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
        private double weight;

        public double Weight
        {
            get { return weight; }
            set { weight = value; RaisePropertyChanged(nameof(Weight)); }
        }
        private int sets;

        public int Sets
        {
            get { return sets; }
            set { sets = value; RaisePropertyChanged(nameof(Sets)); }
        }
        private string muscleGroupName;

        public string MuscleGroupName
        {
            get { return muscleGroupName; }
            set { muscleGroupName = value; RaisePropertyChanged(nameof(MuscleGroupName)); }
        }
        private bool isBusy;

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                RaisePropertyChanged(nameof(IsBusy));
            }
        }


        private ObservableCollection<MuscleGroup> muscleGroups;
        public ObservableCollection<MuscleGroup> MuscleGroups
        {
            get { return muscleGroups; }
            set { muscleGroups = value; RaisePropertyChanged(nameof(MuscleGroups)); }
        }

        private MuscleGroup selectedMuscleGroupEdit;

        public MuscleGroup SelectedMuscleGroupEdit
        {
            get { return selectedMuscleGroupEdit; }
            set { selectedMuscleGroupEdit = value; RaisePropertyChanged(nameof(SelectedMuscleGroupEdit)); }
        }
        private bool isTrainer;

        public bool IsTrainer
        {
            get { return isTrainer; }
            set { isTrainer = value; RaisePropertyChanged(nameof(IsTrainer)); }
        }

        public override async void Init(object initData)
        {
            base.Init(initData);

            Id = (int)initData;

            LoadData.Execute(null);
            IsTrainer = await CheckIfTrainerAsync();

        }
        public ICommand LoadData
        {
            get
            {
                return new Command(async () =>
                {
                    IsBusy = true;
                    var exercise = await _exerciseService.GetExercisesById(Id);
                    Reps = exercise.Reps;
                    Sets = exercise.Sets;
                    Weight = exercise.Weight;
                    Name = exercise.Name;
                    Description = exercise.Description;
                    IsBusy = false;

                });
            }
        }

        public ICommand EditCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await CoreMethods.PushPageModel<ExerciseEditViewModel>(Id, false, true);
                });
            }
        }
        public ICommand DeleteCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (await ConfirmDeleteAsync())
                    {
                        try
                        {
                            await _exerciseService.DeleteExerciseAsync(Id);
                            await CoreMethods.PopPageModel();
                            await CoreMethods.PopPageModel();
                            await CoreMethods.PushPageModel<ExerciseViewModel>();
                        }
                        catch
                        {
                            await CoreMethods.DisplayAlert("Delete failed", "The exercise could not be deleted, please try again", "OK");
                        }
                    }
                });
            }
        }
        private async Task<bool> ConfirmDeleteAsync()
        {
            return await CoreMethods.DisplayAlert("Confirm", "Are you sure you want to delete", "Yes", "No");            
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
