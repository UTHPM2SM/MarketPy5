using MarketPy5.Modelo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace MarketPy5.Vista.VistaClientes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaDetalleHistoria : ContentPage
    {
        Ventas sale;
        public PaginaDetalleHistoria(Ventas _sale)
        {
            InitializeComponent();

            sale = _sale;

            ProductListView.RefreshCommand = new Command(() =>
            {
                OnAppearing();
            });
        }

        protected override void OnAppearing()
        {
            ClientName.Text = sale.ClientName;
            ClientPhone.Text = sale.ClientPhone;
            ClientName.Text = sale.ClientName;
            TotalToPay.Text = sale.TotalToPay;
            PayWay.Text = sale.PayFormat;
            DateTimeText.Text = sale.Date;
            SaleState.Text = sale.State;

            if (sale.State == "Pendiente")
            {
                SaleState.TextColor = Color.DarkRed;
            }
            else
            {
                SaleState.TextColor = Color.Green;
            }

            var ShoppingList = JsonConvert.DeserializeObject<List<Compras>>(sale.ShoppingCar);
            ProductListView.ItemsSource = null;
            ProductListView.ItemsSource = ShoppingList;
            ProductListView.IsRefreshing = false;

            LoadLocation();
        }

        async Task LoadLocation()
        {
            try
            {
                double latitude = Convert.ToDouble(sale.ClientLatitude);
                double longitude = Convert.ToDouble(sale.ClientLongitude);

                var placemarks = await Geocoding.GetPlacemarksAsync(latitude, longitude);
                var placemark = placemarks?.FirstOrDefault();

                ClientLocation.Text = $" ({placemark.CountryCode}) {placemark.CountryName}, {placemark.AdminArea}, {placemark.Locality}";
                Latitude.Text = sale.ClientLatitude;
                Longitude.Text = sale.ClientLongitude;
            }
            catch (Exception ex)
            {
                await DisplayAlert("No ubicacionb", ex.Message, "OK");
            }
        }

        private void BackButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}