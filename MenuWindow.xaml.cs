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
    }
}