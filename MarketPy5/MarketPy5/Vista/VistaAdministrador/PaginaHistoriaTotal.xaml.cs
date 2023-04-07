using MarketPy5.Controlador;
using MarketPy5.Modelo;
using MarketPy5.Vista.VistaClientes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace MarketPy5.Vista.VistaAdministrador
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PaginaHistoriaTotal : ContentPage
	{
        public PaginaHistoriaTotal()
        {
            InitializeComponent();

            HistoryListView.RefreshCommand = new Command(() =>
            {
                OnAppearing();
            });
        }

        protected override async void OnAppearing()
        {
            var historyList = await VentasControlador.GetHistory();
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