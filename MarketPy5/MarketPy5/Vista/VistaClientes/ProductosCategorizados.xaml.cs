using MarketPy5.Modelo;
using MarketPy5.Controlador;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MarketPy5.Vista.VistaClientes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductosCategorizados : ContentPage
    {
        Productos selectedProduct;
        String category;

        [Obsolete]
        public ProductosCategorizados(string _category)
        {
            InitializeComponent();

            category = _category;
            ProductListView.RefreshCommand = new Command(() =>
            {
                OnAppearing();
            });
            CloseModal();

            ClosePopUpModal.GestureRecognizers.Add(new TapGestureRecognizer((view) => CloseModal()));
            LikeButton.GestureRecognizers.Add(new TapGestureRecognizer((view) => LikeMessage()));
        }

        private void LikeMessage()
        {
            DisplayAlert("Calificación", "Gracias por marcar este producto como favorito.", "Un Placer");
        }

        protected override async void OnAppearing()
        {
            var ProductsList = await ProductosControlador.GetAllByCategory(category);
            ProductListView.ItemsSource = null;
            ProductListView.ItemsSource = ProductsList;
            ProductListView.IsRefreshing = false;
        }

        private void ProductListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var product = e.Item as Productos;
            ProductDetailName.Text = product.Name;
            ProductDetailDescription.Text = product.Description;
            ProductDetailId.Text = product.Id;
            ProductDetailImage.Source = product.Image;

            selectedProduct = product;

            ProductDetailPrice.Text = product.Price;

            PopUpModal.IsVisible = true;

        }
        private void OpenModal()
        {
            PopUpModal.IsVisible = true;
        }

        private void CloseModal()
        {
            PopUpModal.IsVisible = false;
            PoundAndUnityText.Text = "1";
        }

        private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var keywords = SearchBar.Text;
            var ProductList = await ProductosControlador.GetAllByCategory(category);
            if (string.IsNullOrEmpty(keywords))
            {
                ProductListView.ItemsSource = ProductList;
            }
            else
            {
                ProductListView.ItemsSource = ProductList.Where(Names => Names.Name.ToLower().Contains(keywords.ToLower()));
            }
        }

        private void CancelBuyButton_Clicked(object sender, EventArgs e)
        {
            CloseModal();
        }

        private void PoundAndUnityText_TextChanged(object sender, TextChangedEventArgs e)
        {
            var actual = ProductDetailPrice.Text;
            ProductDetailPrice.Text = Convert.ToString((Convert.ToDouble(actual) * Convert.ToDouble(PoundAndUnityText.Text)));
        }

        private void BuyButton_Clicked(object sender, EventArgs e)
        {

            if (Preferences.Get("ShoppingCar", "") == "")
            {
                List<Compras> car = new List<Compras>();
                var newShop = new Compras
                {
                    Id = selectedProduct.Id,
                    Image = selectedProduct.Image,
                    ProductName = selectedProduct.Name,
                    PoundUnityPrice = selectedProduct.Price,
                    TotalShop = ProductDetailPrice.Text,
                    Quantity = PoundAndUnityText.Text
                };

                car.Add(newShop);

                String shoppingCar = JsonConvert.SerializeObject(car);

                Preferences.Set("ShoppingCar", shoppingCar);

            }
            else
            {
                String list = Preferences.Get("ShoppingCar", "");
                var items = JsonConvert.DeserializeObject<List<Compras>>(list);
                List<Compras> car = new List<Compras>();
                for (int i = 0; i < items.Count(); i++)
                {
                    car.Add(items[i]);
                }

                var newShop = new Compras
                {
                    Id = selectedProduct.Id,
                    Image = selectedProduct.Image,
                    ProductName = selectedProduct.Name,
                    PoundUnityPrice = selectedProduct.Price,
                    TotalShop = ProductDetailPrice.Text,
                    Quantity = PoundAndUnityText.Text
                };

                car.Add(newShop);
                String shoppingCar = JsonConvert.SerializeObject(car);

                Preferences.Set("ShoppingCar", shoppingCar);
            }
            DisplayAlert("Listo", $"Se agrego {selectedProduct.Name} al carrito.", "OK");
        }

        private void TotalPrice()
        {
            ProductDetailPrice.Text = Convert.ToString((Convert.ToDouble(selectedProduct.Price) * Convert.ToDouble(PoundAndUnityText.Text)));
        }

        private void Add_Clicked(object sender, EventArgs e)
        {
            PoundAndUnityText.Text = Convert.ToString(Convert.ToDouble(PoundAndUnityText.Text) + 1);
            TotalPrice();
        }

        private void Drop_Clicked(object sender, EventArgs e)
        {
            PoundAndUnityText.Text = Convert.ToString(Convert.ToDouble(PoundAndUnityText.Text) - 1);
            TotalPrice();
        }
    }
}