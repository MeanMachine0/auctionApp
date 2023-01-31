using MySql.Data.MySqlClient;
using Mysqlx.Cursor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Bcpg.Attr.ImageAttrib;

namespace auctionApp
{
    public class DataLayer
    {
        private string connectionString = "server=localhost;port=3306;database=auctiondb;uid=root;password=pTHhHFGxB^U5!1UY^22#x0&n;";
        private MySqlConnection connection;
        private string dBPassword;

        private string FormatDateTimeDb(DateTime dateTime)
        {
            const string format = "dd/MM/yyyy HH:mm:ss";
            return DateTime.ParseExact(dateTime.ToString(), format, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
        }

        private TimeOnly GetTimeRemaining(DateTime timeNow, DateTime endTime)
        {
            TimeOnly endsIn = new TimeOnly((endTime - timeNow).Hours, (endTime - timeNow).Minutes, (endTime - timeNow).Seconds);
            return endsIn;
        }

        private void OpenConnection()
        {
            connection = new MySqlConnection(connectionString);
            connection.Open();
        }
        private void Populate(ItemModel model, string query)
        {
            OpenConnection();
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
                        model.TimeOfListing = reader.GetDateTime("timeOfListing");
                        DateTime endTime = reader.GetDateTime("endTime");
                        DateTime timeNow = DateTime.Now;
                        if (timeNow < endTime) { model.TimeRemaining = GetTimeRemaining(timeNow, endTime); }
                        else { model.TimeRemaining = new TimeOnly(0, 0, 0); }
                        model.ReturnsAccepted = reader.GetBoolean("returnsAccepted");
                        model.Description = reader.GetString("information");
                        model.NumBids = reader.GetInt32("numBids");
                    }
                }
            }
            if (model.TimeRemaining == new TimeOnly(0, 0, 0) & model.NumBids > 0 & model.IsSold == false)
            {
                query = $"UPDATE items SET sold = 1 WHERE itemId = {model.ItemId.ToString()}";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            connection.Close();
        }

        public void PopulateItemModel(ItemModel model, int pageNumber)
        {
            string query = $"SELECT * FROM items WHERE itemId = {pageNumber}";
            Populate(model, query);
        }

        public void OnEndOfListing(LoginModel model)
        {
            string query = $"";
        }

        public void ListItem(ItemModel model, int accountId)
        {
            string query = "INSERT INTO items (itemName, currentPrice, postageCost, bidIncrement, state, timeOfListing, endTime, returnsAccepted, information, sellerId) " +
                $"VALUES ('{model.ItemName}', {model.CurrentPrice.ToString()}, {model.PostageCost.ToString()}, {model.BidIncrement.ToString()}, '{model.ItemCondition}', " +
                $"'{FormatDateTimeDb(DateTime.Now)}', '{FormatDateTimeDb(model.EndTime)}', {model.ReturnsAccepted.ToString()}, '{model.Description}', {accountId.ToString()})";
            Debug.Print(query);
            OpenConnection();
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }
            connection.Close();
        }

        public void Search(ItemModel model, string searchText)
        {
            string query = $"SELECT * FROM items WHERE itemName like '%{searchText.Trim()}%' LIMIT 1";
            Populate(model, query);
        }

        internal void SubmitBid(string bidPrice, string itemId, int accountId)
        {
            OpenConnection();
            string query = $"UPDATE items SET currentPrice = {bidPrice}, buyerId = {accountId.ToString()} WHERE itemId = {itemId}";
            Debug.Print(query);
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }
            connection.Close();
        }

        public bool VerifyPassword(LoginModel model)
        {
            OpenConnection();
            string query = $"SELECT accountId, password FROM accounts WHERE username = '{model.Username}'";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        model.AccountId = reader.GetInt32("accountId");
                        dBPassword = reader.GetString("password");
                    };
                }
            }
            connection.Close();
            if (model.Password == dBPassword) 
            {
                return true; 
            } 
            else { return false; };
        }
    }
}
