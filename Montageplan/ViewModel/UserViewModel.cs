using Montageplan.Model;
using System.Collections.Generic;

namespace Montageplan.ViewModel
{
    public class UserViewModel : NotificationModel
    {
        public static List<UserViewModel> CreateViewModels(IEnumerable<User> models)
        {
            List<UserViewModel> viewModels = new List<UserViewModel>();
            foreach (var model in models)
                viewModels.Add(new UserViewModel(model));

            return viewModels;
        }


        private readonly User user;

        public UserViewModel(User u)
        {
            this.user = u;
        }

        public string Username
        {
            get { return this.user.Username; }
        }

        public string UserStatus
        {
            get
            {
                if (this.user.IsAdministrator)
                    return "Administrator";
                return "Nutzer";
            }
        }

        public User GetModel()
        {
            return this.user;
        }

    }
}