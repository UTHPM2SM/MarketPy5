using MarketPy5.Controlador;
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

namespace MarketPy5.Vista.VistaRepartidor
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaTareas : ContentPage
    {
        Ventas sale;
        public PaginaTareas(Ventas _sale)
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

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            Ventas updateSale = new Ventas()
            {
                Id = sale.Id,
                ClientName = sale.ClientName,
                ClientMail = sale.ClientMail,
                ClientPhone = sale.ClientPhone,
                ClientLatitude = sale.ClientLatitude,
                ClientLongitude = sale.ClientLongitude,
                TotalToPay = sale.TotalToPay,
                Detail = sale.Detail,
                Date = sale.Date,
                PayFormat = sale.PayFormat,
                ShoppingCar = sale.ShoppingCar,
                State = "Entregado"
            };

            bool res = await VentasControlador.UpdateState(updateSale);

            if (res)
            {
                await DisplayAlert("Satisfactorio", "La entrega ha sido finalizada.", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Error", "Un error ocurrio en el proceso.", "OK");
            }
        }
    }
}