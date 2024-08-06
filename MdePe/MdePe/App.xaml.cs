using FreshMvvm;
using MdePe.Domain.Services;
using MdePe.Domain.Services.Api;
using MdePe.ViewModels;
using Xamarin.Forms;

namespace MdePe
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            //Register Interfaces with Services
            FreshIOC.Container.Register<IWorkoutService, ApiWorkoutService>().AsSingleton();
            FreshIOC.Container.Register<IExerciseService, ApiExerciseService>().AsSingleton();
            FreshIOC.Container.Register<IUserService, ApiUserService>().AsSingleton();
            FreshIOC.Container.Register<ISecureTokenService, ApiSecureTokenService>().AsSingleton();
            FreshIOC.Container.Register<IMuscleGroupService, ApiMuscleGroupService>().AsSingleton();
            FreshIOC.Container.Register<IGymService, ApiGymsService>().AsSingleton();

            //FreshMvvm navigation container
            MainPage = new FreshNavigationContainer(FreshPageModelResolver.ResolvePageModel<MainViewModel>());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
