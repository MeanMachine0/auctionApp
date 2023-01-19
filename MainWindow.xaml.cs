using System;
using System.Collections.Generic;
using System.Data;
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
        private void selectItemName(int pageNumberInt)
        {
            using MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            using (MySqlCommand command = new MySqlCommand($"SELECT itemName FROM items WHERE itemId = {pageNumberInt}", connection))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        currentItemNameTB.Text = reader.GetString("itemName");
                    }
                }
            }
            connection.Close();
        }
        public MainWindow()
        {
            InitializeComponent();
            using MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            using (MySqlCommand command = new MySqlCommand("SELECT itemName FROM items WHERE itemId = 0", connection))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        currentItemNameTB.Text = reader.GetString("itemName");
                    }
                }
            }
            connection.Close();
        }

        

        private void submit_Click(object sender, RoutedEventArgs e)
        {
            using MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            using (MySqlCommand command = new MySqlCommand("", connection))
            {
                command.ExecuteReader();
            }
            connection.Close();

        }

        private void queryButton_Click(object sender, RoutedEventArgs e)
        {
            using MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            using (MySqlCommand command = new MySqlCommand("DELETE FROM ITEMS WHERE ITEMID IS NULL", connection))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {

                    }
                }
            }
            connection.Close();
        }

        private void pageNext_Click(object sender, RoutedEventArgs e)
        {
            int pageNumberInt = int.Parse(pageNumber.Text) + 1;
            pageNumber.Text = pageNumberInt.ToString();
            selectItemName(pageNumberInt);
        }

        private void pagePrevious_Click(object sender, RoutedEventArgs e)
        {
            int pageNumberInt = int.Parse(pageNumber.Text) - 1;
            pageNumber.Text = pageNumberInt.ToString();
            selectItemName(pageNumberInt);
        }

        private void pageRefresh_Click(object sender, RoutedEventArgs e)
        {
            selectItemName(int.Parse(pageNumber.Text));
        }
    }
}

//string connectionString = "server=localhost;port=3306;database=auctiondb;uid=root;password=pTHhHFGxB^U5!1UY^22#x0&n;";
//using MySqlConnection connection = new MySqlConnection(connectionString);
//connection.Open();
//using (MySqlCommand command = new MySqlCommand("SELECT firstName FROM buyers WHERE buyerId < 3", connection))
//{
//    using (MySqlDataReader reader = command.ExecuteReader())
//    {
//        while (reader.Read())
//        {
//            string firstNamesString = reader.GetString("firstName");
//            string[] firstNames = firstNamesString.Split(" ");
//            foreach (string firstName in firstNames)
//            {
//                output.Text = "Bid submitted.";
//            }
//        }
//   }
//}
//connection.Close();