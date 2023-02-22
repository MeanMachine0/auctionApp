using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        public FilterByWindow()
        {
            InitializeComponent();
            
            if (App.Current.Properties["filterByActive"] is null)
            {
                App.Current.Properties["filterByActive"] = true;
                App.Current.Properties["filterByNotActive"] = true;
                App.Current.Properties["filterByIsSold"] = true;
                App.Current.Properties["filterByIsNotSold"] = true;
                App.Current.Properties["filterByLessThan"] = "0";
                App.Current.Properties["filterByGreaterThan"] = "9999999999";
                App.Current.Properties["filterByIsNew"] = true;
                App.Current.Properties["filterByIsExcellent"] = true;
                App.Current.Properties["filterByIsGood"] = true;
                App.Current.Properties["filterByIsUsed"] = true;
                App.Current.Properties["filterByIsPartsOnly"] = true;
                App.Current.Properties["filterByAreReturnsAccepted"] = true;
                App.Current.Properties["filterByAreReturnsNotAccepted"] = true;
            }

            active.IsChecked = (bool)App.Current.Properties["filterByActive"];
            notActive.IsChecked = (bool)App.Current.Properties["filterByNotActive"];
            isSold.IsChecked = (bool)App.Current.Properties["filterByIsSold"];
            isNotSold.IsChecked = (bool)App.Current.Properties["filterByIsNotSold"];
            lessThan.Text = "£" + App.Current.Properties["filterByLessThan"].ToString(); 
            greaterThan.Text = "£" + App.Current.Properties["filterByGreaterThan"].ToString();
            isNew.IsChecked = (bool)App.Current.Properties["filterByIsNew"];
            isExcellent.IsChecked = (bool)App.Current.Properties["filterByIsExcellent"];
            isGood.IsChecked = (bool)App.Current.Properties["filterByIsGood"];
            isUsed.IsChecked = (bool)App.Current.Properties["filterByIsUsed"];
            isPartsOnly.IsChecked = (bool)App.Current.Properties["filterByIsPartsOnly"];
            areReturnsAccepted.IsChecked = (bool)App.Current.Properties["filterByAreReturnsAccepted"];
            areReturnsNotAccepted.IsChecked = (bool)App.Current.Properties["filterByAreReturnsNotAccepted"];
        }

        private void Apply()
        {
            try
            {
                App.Current.Properties["filterByActive"] = active.IsChecked;
                App.Current.Properties["filterByNotActive"] = notActive.IsChecked;
                App.Current.Properties["filterByIsSold"] = isSold.IsChecked;
                App.Current.Properties["filterByIsNotSold"] = isNotSold.IsChecked;
                App.Current.Properties["filterByIsNew"] = isNew.IsChecked;
                App.Current.Properties["filterByIsExcellent"] = isExcellent.IsChecked;
                App.Current.Properties["filterByIsGood"] = isGood.IsChecked;
                App.Current.Properties["filterByIsUsed"] = isUsed.IsChecked;
                App.Current.Properties["filterByIsPartsOnly"] = isPartsOnly.IsChecked;
                App.Current.Properties["filterByAreReturnsAccepted"] = areReturnsAccepted.IsChecked;
                App.Current.Properties["filterByAreReturnsNotAccepted"] = areReturnsNotAccepted.IsChecked;
                if (isNew.IsChecked is false && isExcellent.IsChecked is false && isGood.IsChecked is false && isUsed.IsChecked is false && isPartsOnly.IsChecked is false)
                {
                    App.Current.Properties["dialog"] = "Please select at least one condition.";
                    openDialog();
                }
                else if (float.Parse(greaterThan.Text.Replace("£", "")) >= (float.Parse(lessThan.Text.Replace("£", ""))))
                {
                    App.Current.Properties["filterByLessThan"] = lessThan.Text.Replace("£", "");
                    App.Current.Properties["filterByGreaterThan"] = greaterThan.Text.Replace("£", "");
                    App.Current.Properties["filtersEnabled"] = true;
                    this.Close();
                }
                else
                {
                    App.Current.Properties["dialog"] = "Error: the less than current price value is greater than the greater than current price value!";
                    openDialog();
                }
            }
            catch
            {
                App.Current.Properties["dialog"] = "Error: one or more fields are invalid!";
                openDialog();
            }
        }

        private void border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) { this.DragMove(); }
        }

        private void apply_Click(object sender, RoutedEventArgs e)
        {
            Apply();
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void keepPound(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text.Length > 0 && textBox.Text[0] != '£')
            {
                textBox.Text = "£" + textBox.Text.Substring(1);
            }

            if (textBox.SelectionStart == 0)
            {
                textBox.SelectionStart = 1;
            }

            if (textBox.Text.Length > 1 && !char.IsDigit(textBox.Text[1]))
            {
                textBox.Text = textBox.Text.Remove(1, 1);
                textBox.SelectionStart = 1;
            }
        }

        private void openDialog()
        {
            DialogWindow dialogWindow = new DialogWindow();
            dialogWindow.ShowDialog();
        }

        private void restrictToMoney(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9£.]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void active_Click(object sender, RoutedEventArgs e)
        {
            if (active.IsChecked is false) { notActive.IsChecked = true; }
        }

        private void notActive_Click(object sender, RoutedEventArgs e)
        {
            if (notActive.IsChecked is false) { active.IsChecked = true; }
        }

        private void isSold_Click(object sender, RoutedEventArgs e)
        {
            if (isSold.IsChecked is false) { isNotSold.IsChecked = true; }
        }

        private void isNotSold_Click(object sender, RoutedEventArgs e)
        {
            if (isNotSold.IsChecked is false) { isSold.IsChecked = true; }
        }

        private void areReturnsAccepted_Click(object sender, RoutedEventArgs e)
        {
            if (areReturnsAccepted.IsChecked is false) { areReturnsNotAccepted.IsChecked = true; }
        }

        private void areReturnsNotAccepted_Click(object sender, RoutedEventArgs e)
        {
            if (areReturnsNotAccepted.IsChecked is false) { areReturnsAccepted.IsChecked = true; }
        }
    }
}
