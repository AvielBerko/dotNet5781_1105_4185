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

using BLAPI;

namespace PL
{
    /// <summary>
    /// Interaction logic for LoginDialog.xaml
    /// </summary>
    public partial class LoginDialog : Window
    {
        readonly IBL bl = BLFactory.GetBL("1");

        public LoginDialog()
        {
            InitializeComponent();
        }

        public BO.User User { get; private set; }

        private void SignUpClick(object sender, RoutedEventArgs e)
        {
            if (txbSignUpPassword.Password != txbSignUpConfirm.Password)
                return;

            try
            {
                User = bl.UserSignUp(txbSignUpName.Text, txbSignUpPassword.Password);

                DialogResult = true;
            }
            catch (BO.BadPasswordValidationException)
            {
                MessageBox.Show("Bad Password!");
            }
            catch (BO.BadSignUpException)
            {
                MessageBox.Show("Bad Name!");
            }
        }

        private void LoginClick(object sender, RoutedEventArgs e)
        {
            try
            {
                User = bl.UserAuthentication(txbLoginName.Text, txbLoginPassword.Password);

                DialogResult = true;
            }
            catch (BO.BadAuthenticationException)
            {
                MessageBox.Show(
                    "Name or password is incorrect.\nPlease try again.",
                    "Bad Authentication",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
