using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using App2.Models;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace App2
{
    public partial class MainPage : ContentPage
    {
        private List<ItemModel> items = new List<ItemModel>();
        private DatabaseHelper dbHelper;
        private bool IsDetailsTab = false;

        public MainPage()
        {
            InitializeComponent();

            var dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "items.db3");
            dbHelper = new DatabaseHelper(dbPath);
        }

        // Scan QR code using ZXing
        private async void OnScanClicked(object sender, EventArgs e)
        {
                    txtCode.Text = string.Empty;
            var scanner = new ZXingScannerPage();
            scanner.OnScanResult += async (result) =>
            {
                await Device.InvokeOnMainThreadAsync(async () =>
                {
                    await Navigation.PopAsync();
                    scanner.IsScanning = false;
                    scanner = null;
                    txtCode.Text = result.Text;
                });
            };

            await Navigation.PushAsync(scanner);
        }

        private void OnCodeChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtCode.Text))
                return;

            if ((items != null && items.Any(x => x.Code == txtCode.Text)))
            {
                DisplayAlert("Duplicate", "This code already exists!", "OK");
                txtCode.Text = string.Empty;
            }
            else if (txtCode.Text.Length == 16)
            {
                items.Add(new ItemModel() { Code = txtCode.Text });
                itemsList.ItemsSource = new ObservableCollection<ItemModel>(items);
                ItemCountBtn.Text = items.Count.ToString();
            }
        }

        private async void AddItem(string code)
        {
            var item = new ItemModel { Code = code };
            await dbHelper.SaveItemAsync(item);
        }

        
        private async void LoadData()
        {
            items = await dbHelper.GetItemsAsync(); 
            itemsList.ItemsSource = new ObservableCollection<ItemModel>(items);  
            ItemCountBtn.Text = items.Count.ToString(); 
        }

        private async void OnNavigateToDetailsClicked(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new DetailsPage(dbHelper));
        }

        private async void OnScanButtonClicked(object sender, EventArgs e)
        {
            IsDetailsTab = false;
            items?.ForEach(f => f.IsDetailsTab = IsDetailsTab);
            scanBtn.TextColor = Color.Black;
            detaislBtn.TextColor = Color.Gray;
            scanFrame.IsVisible = true;
        }
        
        private async void OnDetailsButtonClicked(object sender, EventArgs e)
        {
            try
            {
                IsDetailsTab = true;
                items?.ForEach(f=>f.IsDetailsTab = IsDetailsTab);
                scanBtn.TextColor = Color.Gray;
                detaislBtn.TextColor = Color.Black;
                scanFrame.IsVisible = false;
            }
            catch (Exception)
            {
            }
        }
        
        private async void OnReloadButtonClicked(object sender, EventArgs e)
        {
            LoadData();
        }
        
        private async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            dbHelper?.ClearAllItemsAsync();
            items = new List<ItemModel>();
            itemsList.ItemsSource = new ObservableCollection<ItemModel>(items);
            ItemCountBtn.Text = items.Count.ToString();
        }
        
        private async void OnClearButtonClicked(object sender, EventArgs e)
        {
            dbHelper?.ClearAllItemsAsync();
            items = new List<ItemModel>();
            itemsList.ItemsSource = new ObservableCollection<ItemModel>(items);
            ItemCountBtn.Text = items.Count.ToString();
        }
        
        private async void OnSubmitButtonClicked(object sender, EventArgs e)
        {
            try
            {
                foreach (var item in items)
                {
                    AddItem(item.Code);
                }
                items = new List<ItemModel>();
                itemsList.ItemsSource = new ObservableCollection<ItemModel>(items);
                ItemCountBtn.Text = items.Count.ToString();
            }
            catch (Exception)
            {

            }
        }
    }
}

