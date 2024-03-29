﻿using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace auctionApp
{
    /// <summary>
    /// Interaction logic for BuyWindow.xaml
    /// </summary>

    public partial class BuyWindow : Window
    {
        private ItemModel _model;
        private Timer _timer;
        public bool timerBool;

        private void refresh(int pageNumber, string sortBy, bool? ascending)
        {
            if (sortBy == "Item") { sortBy = "itemName"; }
            else if (sortBy == "Current Price") { sortBy = "currentPrice"; }
            else if (sortBy == "Time Remaining") { sortBy = "endTime"; }
            else if (sortBy == "Bids") { sortBy = "numBids"; }
            else { sortBy = "itemId"; }

            string ascendingString = "";
            if (ascending is false) { ascendingString = "DESC"; }
            else { ascendingString = "ASC"; }

            DataLayer dataLayer = new DataLayer();
            dataLayer.PopulateItemModel(_model, pageNumber, sortBy, ascendingString);
            numPages.Text = App.Current.Properties["numPages"].ToString();
        }

        private void numPagesCheck()
        {
            if (int.Parse(numPages.Text) == 0)
            {
                pagePrevious.IsEnabled = false;
                pageNext.IsEnabled = false;
                pageNumber.IsEnabled = false;
                bid.IsEnabled = false;
                submit.IsEnabled = false;
                searchBar.IsEnabled = false;
                search.IsEnabled = false;
                timerBool = false;
                App.Current.Properties["dialog"] = "No items exist under the filter criteria.";
                openDialog();
                filterByMenu_Click(this, new RoutedEventArgs());
            }
            else
            {
                pagePrevious.IsEnabled = true;
                pageNext.IsEnabled = true;
                pageNumber.IsEnabled = true;
                bid.IsEnabled = true;
                submit.IsEnabled = true;
                searchBar.IsEnabled = true;
                search.IsEnabled = true;
                timerBool = true;
            }
        }

        public BuyWindow()
        {
            InitializeComponent();

            _model = new ItemModel();
            DataContext = _model;
            username.Text = App.Current.Properties["username"].ToString();
            if (App.Current.Properties["sortBy"] is null) { sortByMenu.Text = "Id"; }
            else { sortByMenu.Text = App.Current.Properties["sortBy"].ToString(); }

            if (App.Current.Properties["ascendingBool"] is null) { sortByAscending.IsChecked = true; }
            else { sortByAscending.IsChecked = bool.Parse(App.Current.Properties["ascendingBool"].ToString()); }

            pageNumber.Text = App.Current.Properties["selectedPageNuber"].ToString();

            refresh(int.Parse(pageNumber.Text), sortByMenu.Text, sortByAscending.IsChecked);
            _timer = new Timer(1000);
            _timer.Elapsed += OnTimedEvent;
            _timer.AutoReset = false;
            _timer.Enabled = true;
        }

        private void OnTimedEvent(object? sender, ElapsedEventArgs e)
        {
            Debug.Print("Refreshed at {0:HH:mm:ss.fff}", e.SignalTime);
            App.Current.Dispatcher.Invoke(new Action(async () =>
            {
                try 
                {
                    numPagesCheck();
                    refresh(int.Parse(pageNumber.Text), sortByMenu.Text, sortByAscending.IsChecked);
                }
                catch 
                { 
                    App.Current.Properties["dialog"] = "Error: Invalid Page Number!";
                    openDialog();
                    timerBool = false;
                }
                if (timerBool is true) { _timer.Enabled = true; }
            }));
        }

        private void submit_Click(object sender, RoutedEventArgs e)
        {
            int accountId = (int)App.Current.Properties["accountId"];
            try
            {
                if (float.Parse(bid.Text.Replace(" ", "").Substring(1).Trim()) >= (_model.CurrentPrice + _model.BidIncrement) && 
                    _model.TotalSecondsRemaining > 0 && 
                    pageNumber.Text.Trim() != "" && 
                    bid.Text.Replace(" ", "").Split(".")[1].Length <= 2)
                {
                    DataLayer dataLayer = new DataLayer();
                    dataLayer.SubmitBid(bid.Text.Substring(1).Trim(), _model.ItemId.ToString(), accountId);
                    App.Current.Properties["dialog"] = "Bid submitted.";
                    openDialog();
                }
                else if (pageNumber.Text.Trim() == "")
                {
                    App.Current.Properties["dialog"] = "Cannot Submit bid: please enter a page number to select an item.";
                    openDialog();
                    pageNumber.Text = "1";
                    pageNumber.Text = "";
                }
                else if (bid.Text.Substring(1).Trim().Split(".")[1].Length > 2)
                {
                    App.Current.Properties["dialog"] = "Error: bid has been specified to more than two decimal places!";
                    openDialog();
                }
                else if (float.Parse(bid.Text.Substring(1).Trim()) < (_model.CurrentPrice + _model.BidIncrement) && _model.TotalSecondsRemaining > 0)
                {
                    App.Current.Properties["dialog"] = "Cannot submit bid: bid is less than the current price plus the bid increment.";
                    openDialog();
                }
                else
                {
                    App.Current.Properties["dialog"] = "Listing has ended - could not submit bid.";
                    openDialog();
                }
            }
            catch (Exception Ex)
            { 
                App.Current.Properties["dialog"] = "Error: invalid bid!" + Ex.Message; 
                openDialog(); 
            }
        }

        private void pageNext_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (numPages.Text == "0")
                {
                    submit.IsEnabled = false;
                    App.Current.Properties["dialog"] = "No items exist under the current filters.";
                    openDialog();
                    filterByMenu_Click(this, new RoutedEventArgs());
                }
                else
                {
                    submit.IsEnabled = true;
                    int pageNumberInt = int.Parse(pageNumber.Text) + 1;
                    pageNumber.Text = pageNumberInt.ToString();
                    refresh(pageNumberInt, sortByMenu.Text, sortByAscending.IsChecked);
                }
            }
            catch { }
        }

        private void pagePrevious_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (numPages.Text == "0")
                {
                    submit.IsEnabled = false;
                    App.Current.Properties["dialog"] = "No items exist under the current filters.";
                    openDialog();
                    filterByMenu_Click(this, new RoutedEventArgs());
                }
                else
                {
                    submit.IsEnabled = true;
                    int pageNumberInt = int.Parse(pageNumber.Text) - 1;
                    pageNumber.Text = pageNumberInt.ToString();
                    refresh(pageNumberInt, sortByMenu.Text, sortByAscending.IsChecked);
                }
            }
            catch { }
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    submit.Focus();
                    pagePrevious_Click(this, new RoutedEventArgs());
                    e.Handled = true;
                    break;

                case Key.Right:
                    submit.Focus();
                    pageNext_Click(this, new RoutedEventArgs());
                    e.Handled = true;
                    break;
            }
        }

        private void pageNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                submit.Focus();
            }
        }

        private void bid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                submit_Click(this, new RoutedEventArgs());
                submit.Focus();
            }
        }

        private void search_Click(object sender, RoutedEventArgs e)
        {
            if (numPages.Text == "0") 
            {
                App.Current.Properties["dialog"] = "There are no search results to be shown, as no items exist under the current filters.";
                openDialog();
                _timer.Stop();
            }
            else
            {
                App.Current.Properties["sortBy"] = sortByMenu.Text;
                App.Current.Properties["ascendingBool"] = sortByAscending.IsChecked;
                _timer.Stop();
                App.Current.Properties["searchString"] = searchBar.Text;
                SearchWindow searchWindow = new SearchWindow();
                searchWindow.Show();
                this.Close();
            }
        }

        private void searchBar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                search_Click(this, new RoutedEventArgs());
            }
        }

        private void menu_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Properties["sortBy"] = sortByMenu.Text;
            App.Current.Properties["ascendingBool"] = sortByAscending.IsChecked;
            _timer.Stop();
            MenuWindow menuWindow = new MenuWindow();
            menuWindow.Show();
            this.Close();
        }

        private void logout_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Properties["sortBy"] = sortByMenu.Text;
            App.Current.Properties["ascendingBool"] = sortByAscending.IsChecked;
            _timer.Stop();
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private void border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) { this.DragMove(); }
        }

        private void exitApp_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void keepPound(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text.Length > 0 && textBox.Text[0] != '£')
            {
                textBox.Text = "£" + textBox.Text.Substring(1);
            }

            if (textBox.SelectionStart == 0)
            {
                textBox.SelectionStart = 1;
            }

            if (textBox.Text.Length > 1 && !char.IsDigit(textBox.Text[1]))
            {
                textBox.Text = textBox.Text.Remove(1, 1);
                textBox.SelectionStart = 1;
            }
        }

        private void openDialog()
        {
            _timer.Stop();
            DialogWindow dialogWindow = new DialogWindow();
            dialogWindow.ShowDialog();
            _timer.Start();
        }

        private void filterByMenu_Click(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
            FilterByWindow filterByWindow = new FilterByWindow();
            filterByWindow.ShowDialog();
            if (App.Current.Properties["filtersEnabled"] != null) { pageNumber.Text = "1"; }
            refresh(int.Parse(pageNumber.Text), sortByMenu.Text, sortByAscending.IsChecked); ;
            _timer.Start();
        }

        private void sortByMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            pageNumber.Text = "1";
            refresh(int.Parse(pageNumber.Text), sortByMenu.Text, sortByAscending.IsChecked);
        }

        private void sortByAscending_Click(object sender, RoutedEventArgs e)
        {
            pageNumber.Text = "1";
            refresh(int.Parse(pageNumber.Text), sortByMenu.Text, sortByAscending.IsChecked);
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void pageNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (pageNumber.Text == "0") { pageNumber.Text = "1"; }

            if (numPages is not null) 
            {
                if (numPages.Text == "0")
                {
                    pageNumber.Text = "1";
                    App.Current.Properties["dialog"] = "No items exist under the current filters.";
                    openDialog();
                    _timer.Stop();
                }
            } 

            if (pageNumber.Text.Trim() == "") { _timer.Stop(); timerBool = false; }
            else if (_timer is null) { }
            else 
            { 
                _timer.Start(); timerBool = true;
                try { if (int.Parse(pageNumber.Text) > int.Parse(numPages.Text)) { pageNumber.Text = numPages.Text; } }
                catch { }
            }
        }

        private void numPages_TextChanged(object sender, TextChangedEventArgs e)
        {
            numPagesCheck();
        }

        private void restrictToMoney(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9£.]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}