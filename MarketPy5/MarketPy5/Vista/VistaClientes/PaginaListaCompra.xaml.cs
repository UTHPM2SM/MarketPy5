using MarketPy5.Modelo;
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
    public partial class PaginaListaCompra : ContentPage
    {
        Compras selectedProduct;

        [Obsolete]
        public PaginaListaCompra()
        {
            InitializeComponent();

            ProductListView.RefreshCommand = new Command(() =>
            {
                OnAppearing();
            });
            PopUpModal.IsVisible = false;
            ClosePopUpModal.GestureRecognizers.Add(new TapGestureRecognizer((view) => CloseModal()));
        }

        protected override void OnAppearing()
        {
            String list = Preferences.Get("ShoppingCar", "");
            var ShoppingList = JsonConvert.DeserializeObject<List<Compras>>(list);
            ProductListView.ItemsSource = null;
            ProductListView.ItemsSource = ShoppingList;
            ProductListView.IsRefreshing = false;

            Double total = 0.00;

            if (ShoppingList != null)
            {
                for (int i = 0; i < ShoppingList.Count(); i++)
                {
                    total += Convert.ToDouble(ShoppingList[i].TotalShop);
                }
            }

            TotalToPay.Text = total.ToString();
        }

        private void ProductListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var product = e.Item as Compras;
            ProductDetailName.Text = product.ProductName;
            ProductDetailId.Text = product.Id;
            ProductDetailImage.Source = product.Image;
            PoundAndUnityText.Text = product.Quantity;
            ProductDetailPrice.Text = product.TotalShop;

            selectedProduct = product;

            PopUpModal.IsVisible = true;

        }

        private void OpenModal()
        {
            PopUpModal.IsVisible = true;
        }

        private void CloseModal()
        {
            PopUpModal.IsVisible = false;
        }

        private void CancelBuyButton_Clicked(object sender, EventArgs e)
        {
            Preferences.Remove("ShoppingCar");
            Navigation.PopAsync();
        }
        private void TotalPrice()
        {
            ProductDetailPrice.Text = Convert.ToString((Convert.ToDouble(selectedProduct.PoundUnityPrice) * Convert.ToDouble(PoundAndUnityText.Text)));
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

        private void DropProductButton_Clicked(object sender, EventArgs e)
        {
            String list = Preferences.Get("ShoppingCar", "");
            var ShoppingList = JsonConvert.DeserializeObject<List<Compras>>(list);
            ShoppingList.RemoveAll(X => X.Id == ProductDetailId.Text);

            String shoppingCar = JsonConvert.SerializeObject(ShoppingList);

            Preferences.Set("ShoppingCar", shoppingCar);

            CloseModal();

            OnAppearing();
        }

        private void ApplyButton_Clicked(object sender, EventArgs e)
        {
            String list = Preferences.Get("ShoppingCar", "");
            var ShoppingList = JsonConvert.DeserializeObject<List<Compras>>(list);
            ShoppingList.RemoveAll(X => X.Id == ProductDetailId.Text);

            var editItem = new Compras
            {
                Id = selectedProduct.Id,
                Image = selectedProduct.Image,
                ProductName = selectedProduct.ProductName,
                PoundUnityPrice = selectedProduct.PoundUnityPrice,
                TotalShop = ProductDetailPrice.Text,
                Quantity = PoundAndUnityText.Text
            };

            ShoppingList.Add(editItem);

            String shoppingCar = JsonConvert.SerializeObject(ShoppingList);

            Preferences.Set("ShoppingCar", shoppingCar);

            CloseModal();

            OnAppearing();
        }

        private void MakeBuyButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PaginaDetalleCompras());
        }
    }
}