using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
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
                _model.ItemName = itemName.Text;
                _model.CurrentPrice = float.Parse(startingPrice.Text.Replace("£", "").Replace(" ", ""));
                _model.PostageCost = float.Parse(postageCost.Text.Replace("£", "").Replace(" ", ""));
                _model.BidIncrement = float.Parse(bidIncrement.Text.Replace("£", "").Replace(" ", ""));
                _model.ItemCondition = condition.Text;
                _model.EndTime = DateTime.Parse(endDate.Text + " " + endTime.Text);
                _model.ReturnsAccepted = returnsAccepted.IsChecked.Value;
                _model.Description = description.Text;

                DateTime timeNow = DateTime.Now;
                if (itemName.Text.Length > 0 && itemName.Text.Length <= 30 && startingPrice.Text.Length - 1 > 0 && postageCost.Text.Length - 1 > 0 && bidIncrement.Text.Length - 1 > 0 && _model.BidIncrement > 0 && condition.Text.Length > 0 &&
                _model.EndTime > timeNow && description.Text.Length > 0 && description.Text.Length <= 1000)
                {
                    DataLayer dataLayer = new DataLayer();
                    dataLayer.ListItem(_model, (int)App.Current.Properties["accountId"]);
                    App.Current.Properties["dialog"] = $"{_model.ItemName} Listed. You can go to 'My Listings' to view your listed items.";
                    openDialog();
                }
                else if (_model.EndTime <= timeNow) 
                { 
                    App.Current.Properties["dialog"] = "Error: the end of the listing must be set in the future!";
                    openDialog();
                }
                else if (_model.ItemName.Length > 30) 
                {
                    App.Current.Properties["dialog"] = "Error: item name is too long (30 character limit)!";
                    openDialog();
                }
                else if (_model.Description.Length > 1000) 
                {
                    App.Current.Properties["dialog"] = "Error: description is too long (1000 character limit)!";
                    openDialog();
                }
                else if (_model.BidIncrement <= 0) 
                {
                    App.Current.Properties["dialog"] = "Error: Bid Increment must be greater than £0!";
                    openDialog();
                }
                else 
                {
                    App.Current.Properties["dialog"] = "Error: one or more fields are empty!";
                    openDialog();
                }
            }

            catch 
            {
                App.Current.Properties["dialog"] = "Error: one or more fields are invalid!";
                openDialog();
            }
        }

        private void myListings_Click(object sender, RoutedEventArgs e)
        {
            MyListingsWindow myListingsWindow = new MyListingsWindow();
            myListingsWindow.Show();
            this.Close();
        }

        private void exitApp_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) { this.DragMove(); }
        }

        private void keepPound(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text.Length > 0 && textBox.Text[0] != '£')
            {
                textBox.Text = "£" + textBox.Text.Substring(1);
            }
        }

        private void openDialog()
        {
            DialogWindow dialogWindow = new DialogWindow();
            dialogWindow.ShowDialog();
        }
    }
}
