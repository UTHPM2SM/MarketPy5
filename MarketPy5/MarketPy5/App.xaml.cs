using MarketPy5.Vista;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MarketPy5
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();


            if (!string.IsNullOrEmpty(Preferences.Get("MyFirebaseRefreshToken", "")) && Preferences.Get("Level", "") == "C")
            {
                MainPage = new NavigationPage(new Cliente());
            }
            else if (!string.IsNullOrEmpty(Preferences.Get("MyFirebaseRefreshToken", "")) && Preferences.Get("Level", "") == "D")
            {
                MainPage = new NavigationPage(new Repartidor());
            }
            else if (!string.IsNullOrEmpty(Preferences.Get("MyFirebaseRefreshToken", "")) && Preferences.Get("Level", "") == "A")
            {
                MainPage = new NavigationPage(new Administrador());
            }
            else
            {
                MainPage = new NavigationPage(new MainPage());
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
