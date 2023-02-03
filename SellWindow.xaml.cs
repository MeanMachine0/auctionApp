using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            username.Text = App.Current.Properties["username"].ToString();
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
            try
            {
                _model.ItemName = itemNameS.Text;
                _model.CurrentPrice = float.Parse(startingPrice.Text);
                _model.PostageCost = float.Parse(postageCostS.Text);
                _model.BidIncrement = float.Parse(bidIncrementS.Text);
                _model.ItemCondition = conditionS.Text;
                _model.EndTime = DateTime.Parse(endTimeS.Text);
                _model.ReturnsAccepted = returnsAcceptedS.IsChecked.Value;
                _model.Description = descriptionS.Text;
            }

            catch { }

            if (itemNameS.Text.Length > 0 & startingPrice.Text.Length > 0 & postageCostS.Text.Length > 0 & bidIncrementS.Text.Length > 0 & _model.BidIncrement > 0 & conditionS.Text.Length > 0 &
                endTimeS.Text.Length > 0 & descriptionS.Text.Length > 0)
            {
                DataLayer dataLayer = new DataLayer();
                dataLayer.ListItem(_model, (int)App.Current.Properties["accountId"]);
                MessageBox.Show($"{_model.ItemName} Listed. You can go to 'My Listings' to view your listed item(s).");
            }
            else  if (_model.BidIncrement <= 0){ MessageBox.Show("Error: Bid Increment must be greater than £0!"); }
            else { MessageBox.Show("Error: one or more field(s) are empty!"); }
        }

        private void viewMyListings_Click(object sender, RoutedEventArgs e)
        {
            MyListingsWindow myListingsWindow = new MyListingsWindow();
            myListingsWindow.Show();
            this.Close();
        }
    }
}
