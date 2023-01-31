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

namespace auctionApp
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private LoginModel model;
        public LoginWindow()
        {
            InitializeComponent();
            model = new LoginModel();
        }

        private void login_Click(object sender, RoutedEventArgs e)
        {
           model.Username = username.Text;
           model.Password = password.Text;
           DataLayer dataLayer = new DataLayer();
           if (dataLayer.VerifyPassword(model) == true)
           {
                App.Current.Properties["accountId"] = model.AccountId;
                MenuWindow menuWindow = new MenuWindow();
                menuWindow.Show();
                this.Close();
           }
           else { MessageBox.Show("Invalid username and/or password!"); }
        }
    }
}
