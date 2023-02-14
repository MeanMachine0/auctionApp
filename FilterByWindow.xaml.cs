using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace auctionApp
{
    /// <summary>
    /// Interaction logic for FilterByWindow.xaml
    /// </summary>
    public partial class FilterByWindow : Window
    {
        private FilterByModel _model;

        public FilterByWindow()
        {
            InitializeComponent();
            
            _model = new FilterByModel();
            DataContext = _model;

            if (App.Current.Properties["filterByIsSold"] is null)
            {
                _model.IsSold = false;
                _model.LessThan = 0;
                _model.GreaterThan = 9999999;
                _model.IsNew = false;
                _model.IsExcellent = false;
                _model.IsGood = false;
                _model.IsUsed = false;
                _model.IsPartsOnly = false;
                _model.AreReturnsAccepted = false;
            }
            else
            {
                Debug.Print(App.Current.Properties["filterByIsSold"].ToString());
                _model.IsSold = (bool?)App.Current.Properties["filterByIsSold"];
                _model.LessThan = float.Parse(App.Current.Properties["filterByLessThan"].ToString());
                _model.GreaterThan = float.Parse(App.Current.Properties["filterByGreaterThan"].ToString());
                _model.IsNew = (bool?)App.Current.Properties["filterByIsNew"];
                _model.IsExcellent = (bool?)App.Current.Properties["filterByIsExcellent"];
                _model.IsGood = (bool?)App.Current.Properties["filterByIsGood"];
                _model.IsUsed = (bool?)App.Current.Properties["filterByIsUsed"];
                _model.IsPartsOnly = (bool?)App.Current.Properties["filterByIsPartsOnly"];
                _model.AreReturnsAccepted = (bool?)App.Current.Properties["filterByAreReturnsAccepted"];
            }
        }

        private void Apply()
        {
            _model.IsSold = isSold.IsChecked; App.Current.Properties["filterByIsSold"] = _model.IsSold;
            Debug.Print(App.Current.Properties["filterByIsSold"].ToString());
            _model.LessThan = float.Parse(lessThan.Text); App.Current.Properties["filterByLessThan"] = _model.LessThan;
            _model.GreaterThan = float.Parse(greaterThan.Text); App.Current.Properties["filterByGreaterThan"] = _model.GreaterThan;
            _model.IsNew = isNew.IsChecked; App.Current.Properties["filterByIsNew"] = _model.IsNew;
            _model.IsExcellent = isExcellent.IsChecked; App.Current.Properties["filterByIsExcellent"] = _model.IsExcellent;
            _model.IsGood = isGood.IsChecked; App.Current.Properties["filterByIsGood"] = _model.IsGood;
            _model.IsUsed = isUsed.IsChecked; App.Current.Properties["filterByIsUsed"] = _model.IsUsed;
            _model.IsPartsOnly = isPartsOnly.IsChecked; App.Current.Properties["filterByIsPartsOnly"] = _model.IsPartsOnly;
            _model.AreReturnsAccepted = areReturnsAccepted.IsChecked; App.Current.Properties["filterByAreReturnsAccepted"] = _model.AreReturnsAccepted;
        }

        private void border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) { this.DragMove(); }
        }

        private void apply_Click(object sender, RoutedEventArgs e)
        {
            Apply();
            this.Close();
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
