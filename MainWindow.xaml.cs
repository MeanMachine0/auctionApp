using System;
using System.Diagnostics;
using System.Timers;
using System.Windows;
using System.Windows.Input;

namespace auctionApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private ItemModel _model;
        private Timer _timer;


        private void refresh(int pageNumber)
        {
            DataLayer dataLayer = new DataLayer();
            dataLayer.PopulateItemModel(_model, pageNumber);
        }

        public MainWindow()
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
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                refresh(int.Parse(pageNumber.Text));
                _timer.Enabled = true;
            }));

        }

        private void submit_Click(object sender, RoutedEventArgs e)
        {
            if (float.Parse(bid.Text) >= float.Parse(currentPrice.Text.Remove(0, 1)) + float.Parse(bidIncrement.Text.Remove(0, 1)))
            {
                MessageBox.Show("Bid submitted.");
                DataLayer dataLayer = new DataLayer();
                dataLayer.SubmitBid(bid.Text, pageNumber.Text);
            }
            else
            {
                MessageBox.Show("Cannot submit bid: bid is less than the current price plus the bid increment.");
            }
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
                    pageNext_Click(this, new RoutedEventArgs());
                    break;

                case Key.Right:
                    pagePrevious_Click(this, new RoutedEventArgs());
                    break;

                case Key.Enter:
                    submit_Click(this, new RoutedEventArgs());
                    break;
            }
        }
    }
}