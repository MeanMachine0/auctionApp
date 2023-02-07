using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for MenuWindow.xaml
    /// </summary>
   
    public partial class MenuWindow : Window
    {
        public MenuWindow()
        {
            InitializeComponent();
            username.Text = App.Current.Properties["username"].ToString();
            Debug.Print(Colors.LightBlue.ToString());
        }

        private void buy_Click(object sender, RoutedEventArgs e)
        {
            BuyWindow mainwindow = new BuyWindow();
            mainwindow.Show();
            this.Close();
        }

        private void sell_Click(object sender, RoutedEventArgs e)
        {
            SellWindow sellwindow = new SellWindow();
            sellwindow.Show();
            this.Close();
        }

        private void logout_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private void border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void closeApp_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}