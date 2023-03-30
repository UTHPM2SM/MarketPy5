using MarketPy5.Controlador;
using MarketPy5.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MarketPy5.Vista.VistaClientes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaHistoria : ContentPage
    {
        public PaginaHistoria()
        {
            InitializeComponent();

            HistoryListView.RefreshCommand = new Command(() =>
            {
                OnAppearing();
            });
        }

        protected override async void OnAppearing()
        {
            var historyList = await VentasControlador.GetUserHistory(Preferences.Get("Email", ""));
            HistoryListView.ItemsSource = null;
            HistoryListView.ItemsSource = historyList;
            HistoryListView.IsRefreshing = false;
        }

        private void HistoryListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var history = e.Item as Ventas;
            Navigation.PushAsync(new PaginaDetalleHistoria(history));
        }
    }
}