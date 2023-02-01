using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace auctionApp
{
    public class ItemModel : INotifyPropertyChanged
    {
        private int itemId;
        public int ItemId
        {
            get { return itemId; }
            set
            {
                itemId = value;
                RaisePropertyChanged("ItemId");
            }
        }

        private string itemName;
        public string ItemName
        {
            get { return itemName; }
            set
            {
                itemName = value;
                RaisePropertyChanged("ItemName");
            }
        }
        private bool isSold;
        public bool IsSold
        {
            get { return isSold; }
            set
            {
                isSold = value;
                RaisePropertyChanged("IsSold");
            }
        }

        private float currentPrice;
        public float CurrentPrice
        {
            get { return currentPrice; }
            set
            {
                currentPrice = value;
                RaisePropertyChanged("CurrentPrice");
            }
        }

        private float postageCost;
        public float PostageCost
        {
            get { return postageCost; }
            set
            {
                postageCost = value;
                RaisePropertyChanged("PostageCost");
            }
        }

        private string itemCondition;
        public string ItemCondition
        {
            get { return itemCondition; }
            set
            {
                itemCondition = value;
                RaisePropertyChanged("ItemCondition");
            }
        }

        private float bidIncrement;
        public float BidIncrement
        {
            get { return bidIncrement; }
            set
            {
                bidIncrement = value;
                RaisePropertyChanged("BidIncrement");
            }
        }

        private TimeOnly timeRemaining;
        public TimeOnly TimeRemaining
        {
            get { return timeRemaining; }
            set
            {
                timeRemaining = value;
                RaisePropertyChanged("TimeRemaining");
            }
        }

        private DateTime timeOfListing;
        public DateTime TimeOfListing
        {
            get { return timeOfListing; }
            set
            {
                timeOfListing = value;
                RaisePropertyChanged("TimeOfListing");
            }
        }

        private bool returnsAccepted;
        public bool ReturnsAccepted
        {
            get { return returnsAccepted; }
            set
            {
                returnsAccepted = value;
                RaisePropertyChanged("ReturnsAccepted");
            }
        }

        private string description;
        public string Description
            {
                get { return description; }
                set
                {
                    description = value;
                    RaisePropertyChanged("Description");
                }
            }

        private int numBids;

        public int NumBids
        {
            get { return numBids; }
            set 
            {
                numBids = value;
                RaisePropertyChanged("NumBids");
            }
        }

        private DateTime endTime;

        public DateTime EndTime
        {
            get { return endTime; }
            set
            {
                endTime = value;
                RaisePropertyChanged("EndTime");
            }
        }

        private int buyerId;

        public int BuyerId
        {
            get { return buyerId; }
            set
            {
                buyerId = value;
                RaisePropertyChanged("BuyerId");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
