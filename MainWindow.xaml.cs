using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using MySql.Data.MySqlClient;


namespace auctionApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private string connectionString = "server=localhost;port=3306;database=auctiondb;uid=root;password=pTHhHFGxB^U5!1UY^22#x0&n;";
        private Timer _timer;
        private MySqlConnection connection;
        private void openConnection()
        {
            connection = new MySqlConnection(connectionString);
            connection.Open();
        }
        private string getYesNo(int value)
        {
            if (value == 0)
            {
                return "No";
            }
            else
            {
                return "Yes";
            }
        }

        private void refresh(int pageNumber)
        {
            openConnection();
            string query = $"SELECT * FROM items WHERE itemId = {pageNumber}";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        itemName.Text = reader.GetString("itemName");
                        int soldTest = reader.GetInt16("sold");
                        sold.Text = getYesNo(soldTest);
                        currentPrice.Text = "£" + reader.GetString("currentPrice");
                        postageCost.Text = "£" + reader.GetString("postageCost");
                        state.Text = reader.GetString("state");
                        bidIncrement.Text = "£" + reader.GetString("bidIncrement");
                        timeRemaining.Text = reader.GetString("timeremaining");
                        timeOfListing.Text = reader.GetString("timeOfListing");
                        int returnsAcceptedTest = reader.GetInt16("returnsAccepted");
                        returnsAccepted.Text = getYesNo(returnsAcceptedTest);
                        information.Text = reader.GetString("information");                 
                    }
                }
            }
            connection.Close();
        }

        public MainWindow()
        {
            InitializeComponent();
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
                openConnection();
                string query = $"UPDATE items SET currentPrice = {bid.Text} WHERE itemId = {pageNumber.Text}";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.ExecuteReader();
                }
                connection.Close();
                refresh(int.Parse(pageNumber.Text));
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
            catch{}
        }

        private void pagePrevious_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int pageNumberInt = int.Parse(pageNumber.Text) - 1;
                pageNumber.Text = pageNumberInt.ToString();
                refresh(pageNumberInt);
            }
            catch{}
        }

        private void pageRefresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                refresh(int.Parse(pageNumber.Text));
            }
            catch 
            {
                MessageBox.Show("Invalid page number!");
            }
        }
    }
}