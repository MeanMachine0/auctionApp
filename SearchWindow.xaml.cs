using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        SearchListModel searchList = new SearchListModel();

        private void sortBy(string column)
        {
            searchResults.Items.SortDescriptions.Clear();
            searchResults.Items.SortDescriptions.Add(new SortDescription(column, ListSortDirection.Ascending));
            searchResults.Items.Refresh();
        }
        private void _refresh()
        {
            DataLayer dataLayer = new DataLayer();
            if(searchBar.Text != "") { Application.Current.Properties["searchString"] = searchBar.Text; }
            dataLayer.Search(searchList, Application.Current.Properties["searchString"].ToString());
        }

        public SearchWindow()
        {
            InitializeComponent();
            DataContext = searchList;
            _refresh();
            sortBy("ItemName");
            searchBar.Text = Application.Current.Properties["searchString"].ToString();
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            BuyWindow buyWindow = new BuyWindow();
            buyWindow.Show();
            this.Close();
        }

        private void menu_Click(object sender, RoutedEventArgs e)
        {
            MenuWindow menuWindow = new MenuWindow();
            menuWindow.Show();
            this.Close();
        }

        private void logout_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            _refresh();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            DataGridRow row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(dataGrid.SelectedIndex);
            DataGridCell rowColumn = dataGrid.Columns[11].GetCellContent(row).Parent as DataGridCell;
            App.Current.Properties["selectedId"] = ((TextBlock)rowColumn.Content).Text;
            back_Click(this, new RoutedEventArgs());
        }

        private void searchBar_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    _refresh();
                    e.Handled = true;
                    break;
            }
        }
    }
}
