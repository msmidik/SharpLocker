using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using SharpLocker.Annotations;
using SharpLocker.Services;
using SharpLocker.Models;

namespace SharpLocker.ViewModels
{
    class LoginViewModel : INotifyPropertyChanged
    {
        public User User { get; }

        public ImageSource ProfileImage { get; set; }
        public ImageSource BackgroundImage { get; set; }

        public LoginViewModel()
        {
            User = new User
            {
                UserName = UserSettings.GetUsername(),
                DisplayName = UserSettings.GetDisplayName(),
                Password = string.Empty
            };

            ProfileImage = UserSettings.GetProfileImage();
            BackgroundImage = UserSettings.GetLockScreenImage();
        }

        public string UserName
        {
            get => User.UserName;
            set
            {
                if (User.UserName != value)
                {
                    User.UserName = value;
                    GetOnPropertyChanged(nameof(User.UserName));
                }
            }
        }

        public string DisplayName
        {
            get => User.DisplayName;
            set
            {
                if (User.DisplayName != value)
                {
                    User.DisplayName = value;
                    GetOnPropertyChanged(nameof(User.DisplayName));
                }
            }
        }

        public string Password
        {
            get => User.Password;
            set
            {
                if (User.Password != value)
                {
                    User.Password = value;
                    GetOnPropertyChanged(nameof(User.Password));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void GetOnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
