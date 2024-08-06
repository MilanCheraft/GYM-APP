using FreshMvvm;
using MdePe.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MdePe.ViewModels
{
    public class MainViewModel : FreshBasePageModel
    {

        public ICommand GoToLoginCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await CoreMethods.PushPageModel<LoginViewModel>();
                });
            }
        }
        public ICommand GoToRegisterCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await CoreMethods.PushPageModel<RegisterViewModel>();
                });
            }
        }
    }
}
