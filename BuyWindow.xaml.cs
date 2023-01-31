using System;
using System.Diagnostics;
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

        private void refresh(int pageNumber)
        {
            DataLayer dataLayer = new DataLayer();
            dataLayer.PopulateItemModel(_model, pageNumber);
        }

        public BuyWindow()
        {
            InitializeComponent();

            _model = new ItemModel();
            DataContext = _model;

            refresh(int.Parse(pageNumber.Text));
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
                    returnsAccepted.IsEnabled = true;
                    sold.IsEnabled = true;
                    refresh(int.Parse(pageNumber.Text)); 
                    returnsAccepted.IsEnabled = false;
                    sold.IsEnabled = false;
                }
                catch { MessageBox.Show("Invalid Page Number!"); await Task.Delay(1000); }
                _timer.Enabled = true;
            }));

        }

        private void submit_Click(object sender, RoutedEventArgs e)
        {
            int accountId = (int)Application.Current.Properties["accountId"];
            try
            {
                if (float.Parse(bid.Text) >= (_model.CurrentPrice + _model.BidIncrement) & _model.TimeRemaining > new TimeOnly(0, 0, 0))
                {
                    DataLayer dataLayer = new DataLayer();
                    dataLayer.SubmitBid(bid.Text, pageNumber.Text, accountId);
                    MessageBox.Show("Bid submitted.");
                }
                else if (float.Parse(bid.Text) < (_model.CurrentPrice + _model.BidIncrement) & _model.TimeRemaining > new TimeOnly(0, 0, 0))
                {
                    MessageBox.Show("Cannot submit bid: bid is less than the current price plus the bid increment.");
                }
                else
                {
                    MessageBox.Show("Listing has ended. Could not submit bid.");
                }
            }
            catch { MessageBox.Show("Invalid Bid!"); }
        }

        private void pageNext_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int pageNumberInt = int.Parse(pageNumber.Text) + 1;
                pageNumber.Text = pageNumberInt.ToString();
                refresh(pageNumberInt);
            }
            catch { }
        }

        private void pagePrevious_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int pageNumberInt = int.Parse(pageNumber.Text) - 1;
                pageNumber.Text = pageNumberInt.ToString();
                refresh(pageNumberInt);
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

        private void searchBar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Debug.Print($"Performing search for {searchBar.Text}.");
                DataLayer dataLayer = new DataLayer();
                dataLayer.Search(_model, searchBar.Text);
                pageNumber.Text = _model.ItemId.ToString();
            }
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
    }
}