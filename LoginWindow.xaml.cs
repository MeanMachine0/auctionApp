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
using System.Security.Cryptography;
using System.Diagnostics;
using Org.BouncyCastle.Crypto.Parameters;
using System.Timers;

namespace auctionApp
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private LoginModel model;

        public byte[] dbSalt;

        private Timer _timer;

        private string GetHash(string password, byte[] salt)
        {
            using (var hmac = new HMACSHA256(salt))
            {
                byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    builder.Append(hash[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private string Encrypt(string password)
        {
            string hash = GetHash(password, dbSalt);
            return hash;
        }

        public LoginWindow()
        {
            InitializeComponent();

            //App.Current.Properties["autoLoginBool"] = false;
            if (App.Current.Properties["updateTimerBool"] == null)
            {
                _timer = new Timer(30000);
                _timer.Elapsed += OnTimedEvent;
                _timer.AutoReset = false;
                _timer.Enabled = true;
                App.Current.Properties["updateTimerBool"] = false;
            }

            if (App.Current.Properties["autoLoginBool"] == null)
            {
                App.Current.Properties["autoLoginBool"] = true;
            }
            
            dbSalt = Convert.FromBase64String("WiDyv9IM58jXJ0dle5Fwow==");
            model = new LoginModel();
            username.Focus();
            if (App.Current.Properties["autoLoginBool"] is true)
            {
                App.Current.Properties["autoLoginBool"] = false;
                username.Text = "meanmachine";
                password.Password = "egger";
                login_Click(this, new RoutedEventArgs());
            }
        }

        private void OnTimedEvent(object? sender, ElapsedEventArgs e)
        {
            Debug.Print("Updated sold statuses at {0:HH:mm:ss.fff}", e.SignalTime);
            Application.Current.Dispatcher.Invoke(new Action(async () =>
            {
                try
                {
                    DataLayer dataLayer = new DataLayer();
                    dataLayer.UpdateSoldStatus();
                }
                catch { MessageBox.Show("Error: could not update!"); }
                _timer.Enabled = true;
            }));
        }

        private void login_Click(object sender, RoutedEventArgs e)
        {
           model.Username = username.Text;
           model.Password = Encrypt(password.Password);
           App.Current.Properties["username"] = model.Username;
           DataLayer dataLayer = new DataLayer();
           if (dataLayer.VerifyPassword(model) == true)
           {
                App.Current.Properties["accountId"] = model.AccountId;
                App.Current.Properties["selectedId"] = "1";
                MenuWindow menuWindow = new MenuWindow();
                menuWindow.Show();
                this.Close();
           }
           else { MessageBox.Show("Invalid username and/or password!"); }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    login_Click(this, new RoutedEventArgs());
                    e.Handled = true;
                    break;
            }
        }

        private void border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void exitApp_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            passwordPlaceholder.Visibility = string.IsNullOrEmpty(password.Password) ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
