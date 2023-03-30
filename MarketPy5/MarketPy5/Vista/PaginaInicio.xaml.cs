using Firebase.Auth;
using MarketPy5.ModelosVista;
using MarketPy5.Vista.VistaClientes;
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
    public partial class PaginaInicio : ContentPage
    {
        private string WebApiKey = "AIzaSyA33uUzHinDOOitFq-WNed3dlctMJJmjyk";

        [Obsolete]
        public PaginaInicio()
        {
            InitializeComponent();
            BindingContext = new ModeloVistaInicio();

            GetProfileInformationAndRefreshToken();
            CloseModal();

            MenuButton.GestureRecognizers.Add(new TapGestureRecognizer((view) => OpenModal()));
            ClosePopUpModal.GestureRecognizers.Add(new TapGestureRecognizer((view) => CloseModal()));

            MeatCategory.GestureRecognizers.Add(new TapGestureRecognizer((view) => ToProductsPanel("Carnes")));
            VegetablesCategory.GestureRecognizers.Add(new TapGestureRecognizer((view) => ToProductsPanel("Verduras")));
            DrinksCategory.GestureRecognizers.Add(new TapGestureRecognizer((view) => ToProductsPanel("Bebidas")));
            BabiesCategory.GestureRecognizers.Add(new TapGestureRecognizer((view) => ToProductsPanel("Bebes")));

            ToHistoryPageButton.GestureRecognizers.Add(new TapGestureRecognizer((view) => ToHistoryPage()));
            ToInfoPageButton.GestureRecognizers.Add(new TapGestureRecognizer((view) => ToInfoPage()));
            ToShoppingListButton.GestureRecognizers.Add(new TapGestureRecognizer((view) => ToShoppingList()));
        }

        private void ToShoppingList()
        {
            Navigation.PushAsync(new PaginaListaCompra());
            CloseModal();
        }

        private void ToInfoPage()
        {
            Navigation.PushAsync(new PaginaInformacion());
            CloseModal();
        }

        private void ToHistoryPage()
        {
            Navigation.PushAsync(new PaginaHistoria());
            CloseModal();
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

        [Obsolete]
        private void ToProductsPanel(string category)
        {
            Navigation.PushAsync(new ProductosCategorizados(category));
        }

        private void ProfileButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new EditarPerfil());
            CloseModal();
        }
    }
}