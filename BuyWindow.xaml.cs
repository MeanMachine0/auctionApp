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
            pageNumber.Text = App.Current.Properties["selectedId"].ToString();
            username.Text = App.Current.Properties["username"].ToString();

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
                    refresh(int.Parse(pageNumber.Text)); 
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
                string[] timeRemaining = _model.TimeRemaining.Split(":");
                int days = int.Parse(timeRemaining[0]);
                int hours = int.Parse(timeRemaining[1]);
                int minutes = int.Parse(timeRemaining[2]);
                int seconds = int.Parse((timeRemaining[3]));
                if (float.Parse(bid.Text) >= (_model.CurrentPrice + _model.BidIncrement) & _model.TotalSecondsRemaining > 0)
                {
                    DataLayer dataLayer = new DataLayer();
                    dataLayer.SubmitBid(bid.Text, pageNumber.Text, accountId);
                    MessageBox.Show("Bid submitted.");
                }
                else if (float.Parse(bid.Text) < (_model.CurrentPrice + _model.BidIncrement) & _model.TotalSecondsRemaining > 0)
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
                _timer.Stop();
                Debug.Print($"Performing search for {searchBar.Text}.");
                App.Current.Properties["searchString"] = searchBar.Text;
                SearchWindow searchWindow = new SearchWindow();
                searchWindow.Show();
                this.Close();
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

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
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