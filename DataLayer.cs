using MySql.Data.MySqlClient;
using Mysqlx.Cursor;
using MySqlX.XDevAPI.CRUD;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml;
using static Org.BouncyCastle.Bcpg.Attr.ImageAttrib;

namespace auctionApp
{
    public class DataLayer
    {
        private string connectionString = "server=localhost;port=3306;database=auctiondb;uid=root;password=pTHhHFGxB^U5!1UY^22#x0&n;";
        private MySqlConnection connection;
        private string dbHashedPassword;

        private string FormatDateTimeDb(DateTime dateTime)
        {
            const string format = "dd/MM/yyyy HH:mm:ss";
            return DateTime.ParseExact(dateTime.ToString(), format, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
        }

        private string GetTimeRemaining(DateTime timeNow, DateTime endTime)
        {
            int days = (int)(endTime - timeNow).Days;
            int hours = (int)(endTime - timeNow).Hours;
            int mins = (int)(endTime - timeNow).Minutes;
            int secs = (int)(endTime - timeNow).Seconds;

            if (days == 0 && hours == 0 && mins == 0) { return $"{secs}s"; }
            else if (days == 0 && hours == 0) { return $"{mins}m {secs}s"; }
            else if (days == 0) { return $"{hours}h {mins}m"; }
            else { return $"{days}d {hours}h"; }
        }

        private int GetTotalSecondsRemaining(DateTime timeNow, DateTime endTime)
        {
            int totalSecondsRemaining = (int)(endTime - timeNow).TotalSeconds;
            return totalSecondsRemaining;
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
                        DateTime timeNow = DateTime.Now;
                        DateTime endTime = reader.GetDateTime("endTime");
                        model.TimeRemaining = "";
                        model.TimeRemaining = timeNow < endTime ? GetTimeRemaining(timeNow, endTime) : "0s";
                        model.TotalSecondsRemaining = GetTotalSecondsRemaining(timeNow, endTime);
                        model.ReturnsAccepted = reader.GetBoolean("returnsAccepted");
                        model.Description = reader.GetString("information");
                        model.NumBids = reader.GetInt32("numBids");
                    }
                }
            }
            if (model.TimeRemaining == "0s" && model.NumBids > 0 && model.IsSold == false)
            {
                query = $"UPDATE items SET sold = 1 WHERE itemId = {model.ItemId.ToString()}";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            connection.Close();
        }

        public void PopulateItemModel(ItemModel model, int pageNumber, string sortBy, string ascending)
        {
            if (App.Current.Properties["filtersEnabled"] != null)
            {
                List<string> conditionsChecked = new List<string>();
                if (bool.Parse(App.Current.Properties["filterByIsNew"].ToString()) == true) { conditionsChecked.Add("New"); }
                if (bool.Parse(App.Current.Properties["filterByIsExcellent"].ToString()) == true) { conditionsChecked.Add("Excellent"); }
                if (bool.Parse(App.Current.Properties["filterByIsGood"].ToString()) == true) { conditionsChecked.Add("Good"); }
                if (bool.Parse(App.Current.Properties["filterByIsUsed"].ToString()) == true) { conditionsChecked.Add("Used"); }
                if (bool.Parse(App.Current.Properties["filterByIsPartsOnly"].ToString()) == true) { conditionsChecked.Add("Parts Only"); }

                string inStatement = "";
                foreach (string condition in conditionsChecked) { inStatement += $"'{condition}',"; }
                inStatement = inStatement.Substring(0, inStatement.Length - 1);

                string query = $"SELECT * FROM items WHERE sold IN ({App.Current.Properties["filterByIsSold"].ToString()}, {(!bool.Parse(App.Current.Properties["filterByIsNotSold"].ToString())).ToString()}) AND " +
                $"active IN ({App.Current.Properties["filterByActive"].ToString()}, {(!bool.Parse(App.Current.Properties["filterByNotActive"].ToString())).ToString()}) AND " +
                $"currentPrice BETWEEN {App.Current.Properties["filterByLessThan"].ToString()} AND {App.Current.Properties["filterByGreaterThan"].ToString()} AND " +
                $"state IN ({inStatement}) AND returnsAccepted in ({App.Current.Properties["filterByAreReturnsAccepted"].ToString()}, " +
                $"{(!bool.Parse(App.Current.Properties["filterByAreReturnsNotAccepted"].ToString())).ToString()}) " +
                $"ORDER BY {sortBy} {ascending}, itemName ASC, itemId ASC LIMIT 1 OFFSET {pageNumber - 1}";
                Populate(model, query);
            }
            else
            {
                string query = $"SELECT * FROM items ORDER BY {sortBy} {ascending} LIMIT 1 OFFSET {pageNumber - 1}";
                Populate(model, query);
            } 
        }

        public void PopulateMyListings(MyListingsModel model, int accountId, string searchText)
        {
            model.MyListingsList.Clear();
            OpenConnection();
            string query = $"SELECT itemName, sold, currentPrice, bidIncrement, state, postageCost, timeOfListing, endTime, returnsAccepted, numBids, buyerId FROM items WHERE sellerId = {accountId} AND itemName like '%{searchText.Trim()}%' ORDER BY itemName";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {   
                        ItemModel item = new ItemModel();
                        item.ItemName = reader.GetString("itemName");
                        item.IsSold = reader.GetBoolean("sold");
                        item.CurrentPrice = reader.GetFloat("currentPrice");
                        item.PostageCost = reader.GetFloat("postageCost");
                        item.BidIncrement = reader.GetFloat("bidIncrement");
                        item.ItemCondition = reader.GetString("state");
                        item.TimeOfListing = reader.GetDateTime("timeOfListing");
                        item.EndTime = reader.GetDateTime("endTime");
                        item.ReturnsAccepted = reader.GetBoolean("returnsAccepted");
                        item.NumBids = reader.GetInt32("numBids");
                        try { item.BuyerId = reader.GetInt32("buyerId"); }
                        catch { item.BuyerId = null; }
                        model.MyListingsList.Add(item);
                    }
                }
                connection.Close();
            }
        }

        public void ListItem(ItemModel model, int accountId)
        {
            string query = "INSERT INTO items (itemName, currentPrice, postageCost, bidIncrement, state, timeOfListing, endTime, returnsAccepted, information, sellerId) " +
                $"VALUES ('{model.ItemName}', {model.CurrentPrice.ToString()}, {model.PostageCost.ToString()}, {model.BidIncrement.ToString()}, '{model.ItemCondition}', " +
                $"'{FormatDateTimeDb(DateTime.Now)}', '{FormatDateTimeDb(model.EndTime)}', {model.ReturnsAccepted.ToString()}, '{model.Description}', {accountId.ToString()})";
            OpenConnection();
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }
            connection.Close();
        }

        public void Search(SearchListModel model, string searchText)
        {
            
            model.SearchList.Clear();
            OpenConnection();
            string query = "SELECT itemId, information, itemName, sold, currentPrice, bidIncrement, state, timeOfListing, endTime, " + 
               $"returnsAccepted, numBids, postageCost FROM items WHERE itemName like '%{searchText.Trim()}%' ORDER BY itemName";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ItemModel item = new ItemModel();
                        item.ItemId = reader.GetInt32("itemId");
                        item.PostageCost = reader.GetFloat("postageCost");
                        item.ItemName = reader.GetString("itemName");
                        item.IsSold = reader.GetBoolean("sold");
                        item.CurrentPrice = reader.GetFloat("currentPrice");
                        item.BidIncrement = reader.GetFloat("bidIncrement");
                        item.ItemCondition = reader.GetString("state");
                        item.EndTime = reader.GetDateTime("endTime");
                        item.ReturnsAccepted = reader.GetBoolean("returnsAccepted");
                        item.NumBids = reader.GetInt32("numBids");
                        model.SearchList.Add(item);
                    }
                }
                connection.Close();
            }
        }

        internal void SubmitBid(string bidPrice, string itemId, int accountId)
        {
            OpenConnection();
            string query = $"UPDATE items SET currentPrice = {bidPrice}, buyerId = {accountId.ToString()}, numBids = numBids + 1 WHERE itemId = {itemId}";
            using (MySqlCommand command = new MySqlCommand(query, connection)) { command.ExecuteNonQuery(); }
            connection.Close();
        }

        internal void UpdateStatuses()
        {
            string query = "UPDATE items SET sold = 1 WHERE sold = 0 AND endTime < NOW() AND numBids > 0";
            OpenConnection();
            using (MySqlCommand command = new MySqlCommand(query, connection)) { command.ExecuteNonQuery(); }
            query = "UPDATE items SET active = 0 WHERE endTime < NOW()";
            using (MySqlCommand command = new MySqlCommand(query, connection)) { command.ExecuteNonQuery(); }
            connection.Close();
        }

        public bool VerifyPassword(LoginModel model)
        {
            OpenConnection();
            string query = $"SELECT accountId, hashedPassword FROM accounts WHERE username = '{model.Username}'";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        model.AccountId = reader.GetInt32("accountId");
                        dbHashedPassword = reader.GetString("hashedPassword");
                    };
                }
            }
            connection.Close();
            bool boolean = model.Password == dbHashedPassword ? true : false;
            return boolean;
        }
    }
}
