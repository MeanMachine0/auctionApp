﻿using System;
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

        private void _refresh()
        {
            DataLayer dataLayer = new DataLayer();
            App.Current.Properties["searchString"] = searchBar.Text;
            dataLayer.Search(searchList);
        }

        public SearchWindow()
        {
            InitializeComponent();
            DataContext = searchList;
            searchBar.Text = Application.Current.Properties["searchString"].ToString();
            username.Text = Application.Current.Properties["username"].ToString();
            _refresh();
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

        private void exitApp_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            _refresh();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try 
            {
                DataGrid dataGrid = sender as DataGrid;
                DataGridRow row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(dataGrid.SelectedIndex);
                DataGridCell rowColumn = dataGrid.Columns[9].GetCellContent(row).Parent as DataGridCell;
                DataLayer dataLayer = new DataLayer();
                App.Current.Properties["selectedPageNuber"] = dataLayer.GetPageNumber(((TextBlock)rowColumn.Content).Text);
                back_Click(this, new RoutedEventArgs());
            }
            catch { }
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

        private void border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) { this.DragMove(); }
        }
    }
}
