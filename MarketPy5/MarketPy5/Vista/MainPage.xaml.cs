using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Text.RegularExpressions;
using Xamarin.Forms.Xaml;
using MarketPy5.ModelosVista;
using MarketPy5.Vista;

namespace MarketPy5
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        [Obsolete]
        public MainPage()
        {
            InitializeComponent();
            GoToRegister.GestureRecognizers.Add(new TapGestureRecognizer((view) => ToRegister()));
            ForgotPassword.GestureRecognizers.Add(new TapGestureRecognizer((view) => ToRecover()));

            BindingContext = new ModeloVistaLogin();
        }


        private async void profile1_Clicked(object sender, EventArgs e)
        {
            await profile1.ScaleTo(1.2, 100);
            profile1.FadeTo(0, 200);
            await profile1.ScaleTo(0.8, 100);
            profile1.IsVisible = false;
            anim1.IsVisible = true;
            Device.StartTimer(TimeSpan.FromSeconds(2), () =>
            {
                animDo();
                return false;
            });
        }

        public async void animDo()
        {
            anim1.FadeTo(0, 100);
            anim1.IsVisible = false;
            await gridMenu.FadeTo(0, 0);
            gridMenu.IsVisible = true;
            gridMenu.FadeTo(1, 500);
            await gridMenu.ScaleTo(1.2, 1000);
            await gridMenu.RotateXTo(1.4, 100);
            await gridMenu.ScaleTo(1, 100);
            await lblHello.FadeTo(0, 0);
            lblHello.IsVisible = true;
            await lblHello.FadeTo(1, 150);
            await lblHello.FadeTo(1, 300);
            lblHello.FadeTo(0, 300);
            await lblHello.ScaleTo(2, 350);
            await logControlsPanel.FadeTo(0, 0);
            logControlsPanel.IsVisible = true;
            logControlsPanel.FadeTo(1, 300);
            backWallpaper.Source = "background2.jpg";
            await backWallpaper.ScaleTo(0.9, 1200);
        }

        void ToRegister()
        {
            Navigation.PushAsync(new Registros());
        }

        void ToRecover()
        {
           Navigation.PushAsync(new RecuperacionContra());
        }



    }
}
