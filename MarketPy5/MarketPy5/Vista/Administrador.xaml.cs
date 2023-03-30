using Firebase.Auth;
using MarketPy5.ModelosVista;
using MarketPy5.Vista.VistaAdministrador;
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
	public partial class Administrador : ContentPage
	{
        private string WebApiKey = "AIzaSyA33uUzHinDOOitFq-WNed3dlctMJJmjyk";

        [Obsolete]
        public Administrador()
        {
            InitializeComponent();
            BindingContext = new ModeloVistaAdministrador();

            GetProfileInformationAndRefreshToken();
            ProductButton.GestureRecognizers.Add(new TapGestureRecognizer((view) => ToProductsPanel()));
            UserButton.GestureRecognizers.Add(new TapGestureRecognizer((view) => ToUsersPanel()));

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

        private void ToProductsPanel()
        {
            Navigation.PushAsync(new FormularioProducto());
        }

        private void ToUsersPanel()
        {
            Navigation.PushAsync(new FormularioUsuario());
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }

        private void OpenModal()
        {
            PopUpModal.IsVisible = true;
        }

        private void CloseModal()
        {
            PopUpModal.IsVisible = false;
        }
    }
}