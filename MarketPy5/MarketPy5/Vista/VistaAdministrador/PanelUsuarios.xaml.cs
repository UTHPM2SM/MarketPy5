using MarketPy5.Controlador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MarketPy5.Vista.VistaAdministrador
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FormularioUsuario : ContentPage
    {
        public FormularioUsuario()
        {
            InitializeComponent();

            UsersListView.RefreshCommand = new Command(() =>
            {
                OnAppearing();
            });

        }

        protected override async void OnAppearing()
        {
            var UsersList = await UserControlador.GetAllUser();
            UsersListView.ItemsSource = null;
            UsersListView.ItemsSource = UsersList;
            UsersListView.IsRefreshing = false;

        }

        private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var keywords = SearchBar.Text;
            var UsersList = await UserControlador.GetAllUser();
            if (string.IsNullOrEmpty(keywords))
            {
                UsersListView.ItemsSource = UsersList;
            }
            else
            {
                UsersListView.ItemsSource = UsersList.Where(Names => Names.Name.ToLower().Contains(keywords.ToLower())).Where(Names => Names.Email.ToLower().Contains(keywords.ToLower()));
            }
        }
    }
}