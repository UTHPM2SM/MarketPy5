using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketPy5.Controlador;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MarketPy5.Vista.VistaAdministrador
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PanelAgregarProducto : ContentPage
    {
        MediaFile file;

        [Obsolete]
        public PanelAgregarProducto()
        {
            InitializeComponent();

            AddProductImage.GestureRecognizers.Add(new TapGestureRecognizer((view) => TakeProductImage()));
        }

        private async void TakeProductImage()
        {
            await CrossMedia.Current.Initialize();
            try
            {
                file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                {
                    PhotoSize = PhotoSize.Medium
                });
                if (file == null)
                {
                    return;
                }
                ProductImage.Source = ImageSource.FromStream(() =>
                {
                    return file.GetStream();
                });
            }
            catch (Exception ex)
            {

            }
        }

        private async void AddNewProduct()
        {
            String NameProduct = NameProductText.Text;
            String PriceProduct = PriceProductText.Text;
            String DescriptionProduct = DescriptionProductText.Text;
            string CategoryProduct = string.Empty;
            string ImageProduct;
            if (CategoryPicker.SelectedIndex != null && CategoryPicker.SelectedIndex >= 0)
            {
                CategoryProduct = CategoryPicker.Items[CategoryPicker.SelectedIndex];

                if (file == null || string.IsNullOrEmpty(NameProduct) || string.IsNullOrEmpty(PriceProduct) || string.IsNullOrEmpty(DescriptionProduct) || CategoryPicker.SelectedIndex == -1 || CategoryProduct == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Alerta", "Debe llenar todos los campos del formulario.", "OK");
                }
                else
                {
                    ImageProduct = await ProductosControlador.SaveImage(file.GetStream(), Path.GetFileName(file.Path));

                    bool res = await ProductosControlador.AddProduct(NameProduct, PriceProduct, DescriptionProduct, CategoryProduct, ImageProduct);

                    if (res)
                    {
                        await Application.Current.MainPage.DisplayAlert("Satisfactorio", "Producto agregado satisfactoriamente.", "OK");

                        ProductImage.Source = "https://i.ibb.co/tzqnRTG/camera.png";
                        NameProductText.Text = "";
                        PriceProductText.Text = "";
                        DescriptionProductText.Text = "";
                        CategoryPicker.SelectedIndex = -1;
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Fallo de sistema", "No se pudo agregar el producto.", "OK");
                    }
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Alerta", "Debe seleccionar la categoria del producto.", "OK");
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            AddNewProduct();
        }
    }
}