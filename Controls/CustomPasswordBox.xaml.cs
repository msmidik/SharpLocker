using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SharpLocker.Controls
{
    /// <summary>
    /// Interaction logic for CustomPasswordBox.xaml
    /// </summary>
    public partial class CustomPasswordBox : UserControl
    {
        public CustomPasswordBox()
        {
            InitializeComponent();
        }

        public string Password 
        {
            get { return TxtPassword.Password; }
        }

        public event RoutedEventHandler CustomSubmit;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (CustomSubmit != null)
            {
                CustomSubmit(this, new RoutedEventArgs());
            }
        }

        private void TxtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Button_Click(this, new RoutedEventArgs());
            }
        }

        public event RoutedEventHandler PasswordChange;

        private void TxtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (PasswordChange != null)
            {
                PasswordChange(this, new RoutedEventArgs());
            }
        }

    }
}
