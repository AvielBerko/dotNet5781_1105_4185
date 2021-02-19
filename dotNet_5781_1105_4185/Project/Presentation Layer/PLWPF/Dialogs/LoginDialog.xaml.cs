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
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for LoginDialog.xaml
    /// </summary>
    public partial class LoginDialog : Window
    {
        public LoginDialog(LoginViewModel login)
        {
            InitializeComponent();

            DataContext = login;
        }

        private void LoginPasswordChanged(object sender, RoutedEventArgs e)
        {
            var vm = (LoginViewModel)DataContext;
            var passwordBox = (PasswordBox)sender;
            vm.Password = passwordBox.Password;
        }
    }
}
