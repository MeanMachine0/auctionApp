using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace auctionApp
{
    public class FilterByModel : INotifyPropertyChanged
    {
        private bool? active;
        public bool? Active
        {
            get { return active; }
            set
            {
                active = value;
                RaisePropertyChanged("Active");
            }
        }

        private bool? notActive;
        public bool? NotActive
        {
            get { return notActive; }
            set
            {
                notActive = value;
                RaisePropertyChanged("NotActive");
            }
        }

        private bool? isSold;
        public bool? IsSold
        {
            get { return isSold; }
            set
            {
                isSold = value;
                RaisePropertyChanged("IsSold");
            }
        }

        private bool? isNotSold;
        public bool? IsNotSold
        {
            get { return isNotSold; }
            set
            {
                isNotSold = value;
                RaisePropertyChanged("IsNotSold");
            }
        }

        private float lessThan;
        public float LessThan
        {
            get { return lessThan; }
            set
            {
                lessThan = value;
                RaisePropertyChanged("LessThan");
            }
        }

        private float greaterThan;
        public float GreaterThan
        {
            get { return greaterThan; }
            set
            {
                greaterThan = value;
                RaisePropertyChanged("GreaterThan");
            }
        }

        private bool? isNew;
        public bool? IsNew
        {
            get { return isNew; }
            set
            {
                isNew = value;
                RaisePropertyChanged("IsNew");
            }
        }

        private bool? isExcellent;
        public bool? IsExcellent
        {
            get { return isExcellent; }
            set
            {
                isExcellent = value;
                RaisePropertyChanged("IsExcellent");
            }
        }

        private bool? isGood;
        public bool? IsGood
        {
            get { return isGood; }
            set
            {
                isGood = value;
                RaisePropertyChanged("IsGood");
            }
        }

        private bool? isUsed;
        public bool? IsUsed
        {
            get { return isUsed; }
            set
            {
                isUsed = value;
                RaisePropertyChanged("IsUsed");
            }
        }

        private bool? isPartsOnly;
        public bool? IsPartsOnly
        {
            get { return isPartsOnly; }
            set
            {
                isPartsOnly = value;
                RaisePropertyChanged("IsPartsOnly");
            }
        }

        private bool? areReturnsAccepted;
        public bool? AreReturnsAccepted
        {
            get { return areReturnsAccepted; }
            set
            {
                areReturnsAccepted = value;
                RaisePropertyChanged("AreReturnsAccepted");
            }
        }

        private bool? areReturnsNotAccepted;
        public bool? AreReturnsNotAccepted
        {
            get { return areReturnsNotAccepted; }
            set
            {
                areReturnsNotAccepted = value;
                RaisePropertyChanged("AreReturnsNotAccepted");
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
