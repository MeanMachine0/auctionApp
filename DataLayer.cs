using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace auctionApp
{
    public class DataLayer
    {
        private string connectionString = "server=localhost;port=3306;database=auctiondb;uid=root;password=pTHhHFGxB^U5!1UY^22#x0&n;";
        private MySqlConnection connection;

        private void openConnection()
        {
            connection = new MySqlConnection(connectionString);
            connection.Open();
        }

        public void PopulateItemModel(ItemModel model, int pageNumber)
        {
            openConnection();
            string query = $"SELECT * FROM items WHERE itemId = {pageNumber}";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        model.ItemId = reader.GetInt32("itemId");
                        model.ItemName = reader.GetString("itemName");
                        model.IsSold = reader.GetBoolean("sold");
                        model.CurrentPrice = reader.GetFloat("currentPrice");
                        model.PostageCost = reader.GetFloat("postageCost");
                        model.ItemCondition = reader.GetString("state");
                        model.BidIncrement = reader.GetFloat("bidIncrement");
                        DateTime timeRemaining = DateTime.Parse(reader.GetString("timeremaining"));
                        model.TimeRemaining = new TimeOnly(timeRemaining.Hour, timeRemaining.Minute, timeRemaining.Second);
                        model.TimeOfListing = reader.GetDateTime("timeOfListing");
                        model.ReturnsAccepted = reader.GetBoolean("returnsAccepted");
                        model.Description = reader.GetString("information");
                    }
                }
            }
            connection.Close();
        }

        internal void SubmitBid(string bidPrice, string itemId)
        {
            openConnection();
            string query = $"UPDATE items SET currentPrice = {bidPrice} WHERE itemId = {itemId}";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.ExecuteReader();
            }
            connection.Close();
        }
    }
}
