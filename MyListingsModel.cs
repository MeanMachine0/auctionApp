using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace auctionApp
{
    public class MyListingsModel : INotifyPropertyChanged
    {
        public MyListingsModel()
        {
            myListingsList = new ObservableCollection<ItemModel>();
        }

        private ObservableCollection<ItemModel> myListingsList;

        public ObservableCollection<ItemModel> MyListingsList
        {
            get { return myListingsList; }
            set
            {
                myListingsList = value;
                RaisePropertyChanged("MyListingsList");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

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
