using MarketPy5.Modelo;
using MarketPy5.Vista;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MarketPy5.Controlador
{
    class ProductosControlador
    {
        private string WebApiKey = "AIzaSyA33uUzHinDOOitFq-WNed3dlctMJJmjyk";
        public static FirebaseClient firebaseClient = new FirebaseClient("https://proyectosupermercado-728d4-default-rtdb.firebaseio.com/");
        public static FirebaseStorage firebaseStorage = new FirebaseStorage("proyectosupermercado-728d4.appspot.com");
        //==============================================================================

        //==============================================================================
        public static async Task<List<Productos>> GetAllProducts()
        {
            try
            {
                var productsList = (await firebaseClient
                .Child("Products")
                .OnceAsync<Productos>()).Select(item =>
                new Productos
                {
                    Id = item.Key,
                    Name = item.Object.Name,
                    Price = item.Object.Price,
                    Category = item.Object.Category,
                    Description = item.Object.Description,
                    Image = item.Object.Image,
                }).ToList();
                return productsList;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return null;
            }
        }
        //==============================================================================

        //==============================================================================
        public static async Task<bool> AddProduct(string name, string price, string description, string category, string image)
        {
            try
            {
                await firebaseClient
                .Child("Products")
                .PostAsync(new Productos() { Name = name, Price = price, Description = description, Category = category, Image = image });
                return true;
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Alerta", e.Message, "OK");
                return false;
            }
        }
        //==============================================================================

        //==============================================================================
        public static async Task<List<Productos>> GetAllByCategory(string category)
        {
            return (await firebaseClient.Child("Products").OnceAsync<Productos>()).Select(item => new Productos
            {
                Id = item.Key,
                Name = item.Object.Name,
                Price = item.Object.Price,
                Category = item.Object.Category,
                Description = item.Object.Description,
                Image = item.Object.Image,
            }).Where(c => c.Category.ToLower().Contains(category.ToLower())).ToList();
        }


        //==============================================================================

        //==============================================================================
        public async void CreateNewProduct(string name, string price, string description, string category, string image)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(price) || string.IsNullOrEmpty(description) || string.IsNullOrEmpty(category) || string.IsNullOrEmpty(image))
            {
                await Application.Current.MainPage.DisplayAlert("Alerta", "Se requiere llenar todos los campos del formulario.", "OK");
            }
            else
            {
                try
                {
                    await AddProduct(name, price, description, category, image);
                }
                catch (Exception ex)
                {
                }
            }
        }

        //==============================================================================

        //==============================================================================
        public static async Task<bool> DeleteProduct(string id)
        {
            await firebaseClient.Child("Products" + "/" + id).DeleteAsync();
            return true;
        }

        //==============================================================================

        //==============================================================================
        public static async Task<bool> UpdateProduct(Productos product)
        {
            await firebaseClient.Child("Products" + "/" + product.Id).PutAsync(JsonConvert.SerializeObject(product));
            return true;
        }


        //==============================================================================

        //==============================================================================

        public static async Task<string> SaveImage(Stream image, string filename)
        {
            var img = await firebaseStorage.Child("ProductsImages").Child(filename).PutAsync(image);
            return img;
        }
        //==============================================================================

        //==============================================================================

        public static async Task<bool> DeleteImage(string filename)
        {
            await firebaseStorage.Child("ProductsImages").Child(filename).DeleteAsync();
            return true;
        }
    }
}