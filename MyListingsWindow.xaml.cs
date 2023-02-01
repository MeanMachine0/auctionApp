using Org.BouncyCastle.Tls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
    /// Interaction logic for MyListingsWindow.xaml
    /// </summary>
    public partial class MyListingsWindow : Window
    {
        MyListingsModel myListings = new MyListingsModel();
        private void _refresh()
        {
            int accountId = (int)Application.Current.Properties["accountId"];
            DataLayer dataLayer = new DataLayer();
            dataLayer.PopulateMyListings(myListings, accountId);
        }
        public MyListingsWindow()
        {
            InitializeComponent();
            DataContext = myListings;
            _refresh();
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            SellWindow sellWindow = new SellWindow();
            sellWindow.Show();
            this.Close();
        }

        private void menu_Click(object sender, RoutedEventArgs e)
        {
            MenuWindow menuWindow = new MenuWindow();
            menuWindow.Show();
            this.Close();
        }

        private void logout_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            _refresh();
        }
    }
}
