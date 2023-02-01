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
    /// Interaction logic for SellWindow.xaml
    /// </summary>

    public partial class SellWindow : Window
    {
        private ItemModel _model;

        public SellWindow()
        {
            InitializeComponent();

            _model = new ItemModel();
            DataContext = _model;
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

        private void submitListing_Click(object sender, RoutedEventArgs e)
        {
            _model.ItemName = itemNameS.Text;
            _model.CurrentPrice = float.Parse(startingPrice.Text);
            _model.PostageCost = float.Parse(postageCostS.Text);
            _model.BidIncrement = float.Parse(bidIncrementS.Text);
            _model.ItemCondition = conditionS.Text;
            _model.EndTime = DateTime.Parse(endTimeS.Text);
            _model.ReturnsAccepted = returnsAcceptedS.IsChecked.Value;
            _model.Description = descriptionS.Text;
            DataLayer dataLayer = new DataLayer();
            dataLayer.ListItem(_model, (int) App.Current.Properties["accountId"]);
        }

        private void viewMyListings_Click(object sender, RoutedEventArgs e)
        {
            MyListingsWindow myListingsWindow = new MyListingsWindow();
            myListingsWindow.Show();
            this.Close();
        }
    }
}
