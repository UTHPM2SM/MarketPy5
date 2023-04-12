using MarketPy5.Modelo;
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
    public partial class FormularioProducto : ContentPage
    {
        Productos selectedProduct;
        [Obsolete]
        public FormularioProducto()
        {
            InitializeComponent();

            ProductListView.RefreshCommand = new Command(() =>
            {
                OnAppearing();
            });
            AddNewProduct.GestureRecognizers.Add(new TapGestureRecognizer((view) => ToAddProductPanel()));
            ClosePopUpModal.GestureRecognizers.Add(new TapGestureRecognizer((view) => CloseModal()));

            PopUpModal.IsVisible = false;
        }

        protected override async void OnAppearing()
        {
            var ProductsList = await ProductosControlador.GetAllProducts();
            ProductListView.ItemsSource = null;
            ProductListView.ItemsSource = ProductsList;
            ProductListView.IsRefreshing = false;

        }
        void ToAddProductPanel()
        {
            Navigation.PushAsync(new PanelAgregarProducto());
        }

        private void ProductListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var product = e.Item as Productos;
            ProductDetailName.Text = product.Name;
            ProductDetailDescription.Text = product.Description;
            ProductDetailPrice.Text = product.Price + " Lps";
            ProductDetailId.Text = product.Id;
            ProductDetailImage.Source = product.Image;

            selectedProduct = product;

            PopUpModal.IsVisible = true;
        }
        private void CloseModal()
        {
            PopUpModal.IsVisible = false;
        }

        private async void BorrarProducto()
        {
            String id = ProductDetailId.Text;
            bool res = await ProductosControlador.DeleteProduct(id);

            if (res)
            {
                await DisplayAlert("Satisfactorio", "Se elimino el producto.", "OK");
                CloseModal();
            }
            else
            {
                await DisplayAlert("Fallo del sistema", "No se pudo eliminar el producto.", "OK");
            }
        }

        private void UpdateButton_Clicked(object sender, EventArgs e)
        {
            CloseModal();
            Navigation.PushAsync(new PanelEditarProducto(selectedProduct));
        }

        private void DeleteButton_Clicked(object sender, EventArgs e)
        {
            BorrarProducto();
        }

        private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var keywords = SearchBar.Text;
            var ProductList = await ProductosControlador.GetAllProducts();
            if (string.IsNullOrEmpty(keywords))
            {
                ProductListView.ItemsSource = ProductList;
            }
            else
            {
                ProductListView.ItemsSource = ProductList.Where(Names => Names.Name.ToLower().Contains(keywords.ToLower()));
            }
        }
    }
}