﻿using Firebase.Auth;
using MarketPy5.Controlador;
using MarketPy5.Modelo;
using MarketPy5.ModelosVista;
using MarketPy5.Vista.VistaRepartidor;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MarketPy5.Vista
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Repartidor : ContentPage
	{
        private string WebApiKey = "AIzaSyA33uUzHinDOOitFq-WNed3dlctMJJmjyk";
        public Repartidor()
        {
            InitializeComponent();
            BindingContext = new ModeloVistaRepartidor();

            HistoryListView.RefreshCommand = new Command(() =>
            {
                OnAppearing();
            });

            GetProfileInformationAndRefreshToken();

            MenuButton.GestureRecognizers.Add(new TapGestureRecognizer((view) => OpenModal()));
            ClosePopUpModal.GestureRecognizers.Add(new TapGestureRecognizer((view) => CloseModal()));

            PopUpModal.IsVisible = false;
        }

        public async void GetProfileInformationAndRefreshToken()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebApiKey));
            try
            {
                //This is the saved firebaseauthentication that was saved during the time of login
                var savedfirebaseauth = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences.Get("MyFirebaseRefreshToken", ""));
                //Here we are Refreshing the token
                var RefreshedContent = await authProvider.RefreshAuthAsync(savedfirebaseauth);
                Preferences.Set("MyFirebaseRefreshToken", JsonConvert.SerializeObject(RefreshedContent));
                //Now lets grab user information
                UserName.Text = Preferences.Get("Username", "UserNoDefined");
                UserEmail.Text = Preferences.Get("Email", "NoEmailFounded");
                UserPhone.Text = Preferences.Get("Phone", "00000000");
                UserImage.Source = Preferences.Get("Image", "https://i.ibb.co/vhh0Gkj/users.png");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await Application.Current.MainPage.DisplayAlert("Alerta", "La sesión ya ha terminado", "OK");
            }
        }

        private void OpenModal()
        {
            PopUpModal.IsVisible = true;
        }

        private void CloseModal()
        {
            PopUpModal.IsVisible = false;
        }

        protected override async void OnAppearing()
        {
            var historyList = await VentasControlador.GetPendingSales();
            HistoryListView.ItemsSource = null;
            HistoryListView.ItemsSource = historyList;
            HistoryListView.IsRefreshing = false;
        }

        private void HistoryListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var sale = e.Item as Ventas;
            Navigation.PushAsync(new PaginaTareas(sale));
        }
    }
}