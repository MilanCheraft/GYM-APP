using FreshMvvm;
using MdePe.Domain.Models;
using MdePe.Domain.Services;
using MdePe.Domain.Validators;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MdePe.ViewModels
{
    public class ExerciseEditViewModel : FreshBasePageModel
    {
        protected readonly IExerciseService _exerciseService;
        protected readonly IMuscleGroupService _muscleGroupService;

        public ExerciseEditViewModel(IExerciseService exerciseService, IMuscleGroupService muscleGroupService)
        {
            _exerciseService = exerciseService;
            _muscleGroupService = muscleGroupService;
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
        private int muscleGroupIndex;

        public int MuscleGroupIndex
        {
            get { return muscleGroupIndex; }
            set { muscleGroupIndex = value; RaisePropertyChanged(nameof(MuscleGroupIndex)); }
        }

        public override void Init(object initData)
        {
            base.Init(initData);

            Id = (int)initData;

            LoadData.Execute(null);
        }
        public ICommand CancelEditCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await CoreMethods.PopPageModel();
                });
            }
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
                    MuscleGroups = new ObservableCollection<MuscleGroup>(await _muscleGroupService.GetMuscleGroups());
                    var selectedMuscleGroup = MuscleGroups.FirstOrDefault(mg => mg.Id == exercise.MuscleGroupName.Id);
                    SelectedMuscleGroupEdit = selectedMuscleGroup;
                    MuscleGroupIndex = MuscleGroups.IndexOf(selectedMuscleGroup);
                    Name = exercise.Name;
                    Description = exercise.Description;
                    IsBusy = false;

                });
            }
        }
        public ICommand SaveEditCommand
        {
            get
            {
                return new Command(async () =>
                {
                    Exercise exercise = new Exercise();
                    exercise.Id = Id;
                    exercise.Description = Description;
                    exercise.MuscleGroupName = SelectedMuscleGroupEdit;
                    exercise.Reps = Reps;
                    exercise.Sets = Sets;
                    exercise.Name = Name;
                    exercise.Weight = Weight;

                    if (await Validate(exercise))
                    {
                        if (await _exerciseService.UpdateExerciseAsync(exercise))
                        {
                            await CoreMethods.PopPageModel();
                            await CoreMethods.PushPageModel<ExerciseDetailViewModel>(Id, false, true);
                        }
                        else
                        {
                            await CoreMethods.DisplayAlert("Edit", "Exercise failed to update", "OK");
                        }
                    }
                });
            }
        }
        private async Task<bool> Validate(Exercise exercise)
        {
            var validator = new ExerciseValidator();
            var result = validator.Validate(exercise);

            if (result.IsValid)
            {
                return true;
            }
            foreach (var error in result.Errors)
            {
                if (error.PropertyName == nameof(Description))
                {
                    await CoreMethods.DisplayAlert("Description", error.ErrorMessage, "OK");
                }
                if (error.PropertyName == nameof(Reps))
                {
                    await CoreMethods.DisplayAlert("Reps", error.ErrorMessage, "OK");
                }
                if (error.PropertyName == nameof(Name))
                {
                    await CoreMethods.DisplayAlert("Name", error.ErrorMessage, "OK");
                }
                if (error.PropertyName == nameof(Weight))
                {
                    await CoreMethods.DisplayAlert("Weight", error.ErrorMessage, "OK");
                }
            }
            return false;
        }
    }
}
