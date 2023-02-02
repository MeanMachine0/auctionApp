using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace auctionApp
{
    public class SearchListModel : INotifyPropertyChanged
    {
        public SearchListModel() { searchList = new ObservableCollection<ItemModel>(); }

        private ObservableCollection<ItemModel> searchList;

        public ObservableCollection<ItemModel> SearchList
        {
            get { return searchList; }
            set
            {
                searchList = value;
                RaisePropertyChanged("SearchList");
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
