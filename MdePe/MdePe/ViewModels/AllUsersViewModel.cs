using FreshMvvm;
using MdePe.Domain.Models;
using MdePe.Domain.Services;
using MdePe.Infrastructure.Dto.Users;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace MdePe.ViewModels
{
    public class AllUsersViewModel : FreshBasePageModel
    {
        protected readonly IUserService _userService;

        public AllUsersViewModel(IUserService userService)
        {
            _userService = userService;
        }
        private BaseUserdto selectedUser;

        public BaseUserdto SelectedUser
        {
            get => selectedUser;
            set
            {
                selectedUser = value;
                GoToUserDetailPage.Execute(null);
            }
        }
        private bool isBusy;

        public bool IsBusy
        {
            get { return isBusy; }
            set { isBusy = value; RaisePropertyChanged(nameof(IsBusy)); }
        }

        private ObservableCollection<BaseUserdto> allUsers;
        public ObservableCollection<BaseUserdto> AllUsers
        {
            get { return allUsers; }
            set { allUsers = value; RaisePropertyChanged(nameof(AllUsers)); }
        }
        public override void Init(object initData)
        {
            IsBusy = true;
            base.Init(initData);
            LoadUsers();
            IsBusy = false;
        }
        private async void LoadUsers()
        {
            var users = await _userService.GetUsersAsync();
            AllUsers = new ObservableCollection<BaseUserdto>(users.OrderBy(u => u.Name));
        }
        public ICommand GoToUserDetailPage
        {
            get
            {
                return new Command(async () =>
                {
                    if (SelectedUser != null)
                        await CoreMethods.PushPageModel<UserDetailViewModel>(SelectedUser.Name, false, true);
                });
            }
        }
    }
}
