﻿using Creating_XML.src;
using Creating_XML.src.db;
using Creating_XML.src.db.models;
using Creating_XML.src.db.tables;
using Creating_XML.src.objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Creating_XML.windows
{
    public partial class MainWindow : Window
    {
        private int lastNumber;

        private int maxItemsOnPage = 20;

        private int currentPage = 1;

        /// <summary>
        /// Select file before work (open window).
        /// </summary>
        /// <see cref="OpenFileWindow()"/>
        public MainWindow()
        {
            InitializeComponent();
            OpenFileWindow();
            // TODO Get all info (currencies, categories, etc)
        }

        /// <summary>
        /// OpenFileWindow for select a file and connect to the Database.
        /// </summary>
        /// <see cref="SelectFileWindow"/>
        private void OpenFileWindow()
        {
            Hide();

            var fileWindow = new SelectFileWindow();
            fileWindow.ShowDialog();

            if (fileWindow.IsOpened)
            {
                Show();
                GUI();
            }
            else
                Close();
        }

        /// <summary>
        /// Update GUI (Fill data).
        /// </summary>
        private void GUI()
        {
            UpdateListView(fSearch.Text, maxItemsOnPage, currentPage);
        }

        /// <summary>
        /// Query to Database and update ListView.
        /// </summary>
        private async void UpdateListView(string search, int take, int page)
        {
            if (!Database.HasConnection())
                return;
            
            var collection = await Task.Run(() =>
            {
                try
                {
                    //var list = Database.List<OfferTable>();

                    //if (!string.IsNullOrWhiteSpace(search))
                    //    list.Where(s => s.Name.Contains(search));

                    //return list.Take(take).Skip(take * (page - 1)).ToList();

                    var q = Database.Query<OfferObject>(
                          "SELECT OT.*, CatT.Name as CategoryName, CurT.Name as CurrencyName," +
                          " CurT.Rate as CurrencyRate, VenT.Name as VendorName"
                        + " FROM OfferTable OT "
                        + " INNER JOIN CategoryTable CatT ON OT.CategoryId = CatT.Id"
                        + " INNER JOIN CurrencyTable CurT ON OT.CurrencyId = CurT.Id"
                        + " INNER JOIN VendorTable VenT ON OT.VendorId = VenT.Id"
                    ).ToList();

                    return q;
                }
                catch { return null; }
            });

            if (collection == null)
            {
                MessageBox.Show("Файл повреждён");
                Settings.DeleteFileUri(Project.FileUri);
                OpenFileWindow();
                return;
            }

            listView.ItemsSource = collection;
            listView.Items.Refresh();
        }

        /// <summary>
        /// After got focus = clear the text.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fCurrentPage_GotFocus(object sender, RoutedEventArgs e)
        {
            lastNumber = int.Parse(fCurrentPage.Text);
            fCurrentPage.Text = string.Empty;
        }

        /// <summary>
        /// Check Input.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fCurrentPage_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(fCurrentPage.Text, out int result) || result < 1)
                fCurrentPage.Text = lastNumber.ToString();
        }

        /// <summary>
        /// Accept only numbers.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fCurrentPage_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = RegexHelper.IsOnlyNumbers(e.Text);
        }

        /// <summary>
        /// Change text => Update GUI.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fCurrentPage_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(fMaxItemsOnPage.Text, out int result) && result > 0)
            {
                maxItemsOnPage = result;
                GUI();
            }
        }

        /// <summary>
        /// After got focus = clear the text.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fMaxItemsOnPage_GotFocus(object sender, RoutedEventArgs e)
        {
            lastNumber = int.Parse(fMaxItemsOnPage.Text);
            fMaxItemsOnPage.Text = string.Empty;
        }

        /// <summary>
        /// Check Input.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fMaxItemsOnPage_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(fMaxItemsOnPage.Text, out int result) || result < 1)
                fMaxItemsOnPage.Text = lastNumber.ToString();
        }

        /// <summary>
        /// Accept only numbers.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fMaxItemsOnPage_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = RegexHelper.IsOnlyNumbers(e.Text);
        }

        /// <summary>
        /// Change text => Update GUI.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fMaxItemsOnPage_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(fMaxItemsOnPage.Text, out int result) && result > 0)
            {
                maxItemsOnPage = result;
                GUI();
            }
        }

        /// <summary>
        /// Open OfferWindow for add a new offer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddOffer_Click(object sender, RoutedEventArgs e)
        {
            new OfferWindow().ShowDialog();
            GUI();
        }

        /*
         * |-------------------------------------------
         * | Menu items.
         * |-------------------------------------------
         * |
         */

        /// <summary>
        /// Open window for select file.
        /// </summary>
        /// <see cref="OpenFileWindow()"/>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileWindow();
        }

        /// <summary>
        /// Close the program.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Open VendorWidnow for add a new vendor.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemVendor_Click(object sender, RoutedEventArgs e)
        {
            new VendorWindow().ShowDialog();
        }

        /// <summary>
        /// Open CategoryWindow for add a new currency.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemCurrency_Click(object sender, RoutedEventArgs e)
        {
            new CurrencyWindow().ShowDialog();
        }

        /// <summary>
        /// Open ShowWindow for config info about shop.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemShop_Click(object sender, RoutedEventArgs e)
        {
            new ShopWindow().ShowDialog();
        }

        /// <summary>
        /// Open CategoryWindow for add a new category.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemCategory_Click(object sender, RoutedEventArgs e)
        {
            new CategoryWindow().ShowDialog();
        }
    }
}
