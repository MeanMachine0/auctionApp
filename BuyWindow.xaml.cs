using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
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
        }

        public BuyWindow()
        {
            InitializeComponent();

            _model = new ItemModel();
            DataContext = _model;
            pageNumber.Text = App.Current.Properties["selectedId"].ToString();
            username.Text = App.Current.Properties["username"].ToString();

            refresh(int.Parse(pageNumber.Text), sortByMenu.Text, sortByAscending.IsChecked);
            _timer = new Timer(1000);
            _timer.Elapsed += OnTimedEvent;
            _timer.AutoReset = false;
            _timer.Enabled = true;
        }

        private void OnTimedEvent(object? sender, ElapsedEventArgs e)
        {
            Debug.Print("Refreshed at {0:HH:mm:ss.fff}", e.SignalTime);
            Application.Current.Dispatcher.Invoke(new Action(async () =>
            {
                try 
                {
                    refresh(int.Parse(pageNumber.Text), sortByMenu.Text, sortByAscending.IsChecked);
                }
                catch 
                { 
                    Application.Current.Properties["dialog"] = "Error: Invalid Page Number!";
                    openDialog(); 
                    await Task.Delay(1000); 
                }
                _timer.Enabled = true;
            }));
        }

        private void submit_Click(object sender, RoutedEventArgs e)
        {
            int accountId = (int)Application.Current.Properties["accountId"];
            try
            {
                if (float.Parse(bid.Text.Replace("£", "").Replace(" ", "")) >= (_model.CurrentPrice + _model.BidIncrement) && _model.TotalSecondsRemaining > 0)
                {
                    DataLayer dataLayer = new DataLayer();
                    dataLayer.SubmitBid(bid.Text.Replace("£", "").Replace(" ", ""), pageNumber.Text, accountId);
                    Application.Current.Properties["dialog"] = "Bid submitted.";
                    openDialog();
                }
                else if (float.Parse(bid.Text.Replace("£", "").Replace(" ", "")) < (_model.CurrentPrice + _model.BidIncrement) && _model.TotalSecondsRemaining > 0)
                {
                    Application.Current.Properties["dialog"] = "Cannot submit bid: bid is less than the current price plus the bid increment.";
                    openDialog();
                }
                else 
                { 
                    Application.Current.Properties["dialog"] = "Listing has ended - could not submit bid."; 
                    openDialog(); 
                }
            }
            catch 
            { 
                Application.Current.Properties["dialog"] = "Error: invalid bid!"; 
                openDialog(); 
            }
        }

        private void pageNext_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int pageNumberInt = int.Parse(pageNumber.Text) + 1;
                pageNumber.Text = pageNumberInt.ToString();
                refresh(pageNumberInt, sortByMenu.Text, sortByAscending.IsChecked);
            }
            catch { }
        }

        private void pagePrevious_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int pageNumberInt = int.Parse(pageNumber.Text) - 1;
                pageNumber.Text = pageNumberInt.ToString();
                refresh(pageNumberInt, sortByMenu.Text, sortByAscending.IsChecked);
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
            _timer.Stop();
            Debug.Print($"Performing search for {searchBar.Text}.");
            App.Current.Properties["searchString"] = searchBar.Text;
            SearchWindow searchWindow = new SearchWindow();
            searchWindow.Show();
            this.Close();
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
            _timer.Stop();
            MenuWindow menuWindow = new MenuWindow();
            menuWindow.Show();
            this.Close();
        }

        private void logout_Click(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
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
        }

        private void openDialog()
        {
            DialogWindow dialogWindow = new DialogWindow();
            dialogWindow.ShowDialog();
        }

        private void sortByAscending_Click(object sender, RoutedEventArgs e)
        {
            sortByAscending.IsChecked = true;
            sortByDescending.IsChecked = false;
        }

        private void sortByDescending_Click(object sender, RoutedEventArgs e)
        {
            sortByAscending.IsChecked = false;
            sortByDescending.IsChecked = true;
        }
    }
}