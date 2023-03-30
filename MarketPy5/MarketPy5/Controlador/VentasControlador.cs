using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using MarketPy5.Modelo;

namespace MarketPy5.Controlador
{
     class VentasControlador
    {
        private string WebApiKey = "AIzaSyA33uUzHinDOOitFq-WNed3dlctMJJmjyk";
        public static FirebaseClient firebase = new FirebaseClient("https://proyectosupermercado-728d4-default-rtdb.firebaseio.com/");


        //==============================================================================

        //==============================================================================
        public static async Task<bool> AddSale(Ventas sale)
        {
            try
            {
                await firebase
                .Child("Sales")
                .PostAsync(sale);
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
        public static async Task<List<Ventas>> GetUserHistory(string email)
        {
            return (await firebase.Child(nameof(Ventas)).OnceAsync<Ventas>()).Select(item => new Ventas
            {
                Id = item.Key,
                ClientName = item.Object.ClientName,
                ClientMail = item.Object.ClientMail,
                ClientPhone = item.Object.ClientPhone,
                ClientLatitude = item.Object.ClientLatitude,
                ClientLongitude = item.Object.ClientLongitude,
                TotalToPay = item.Object.TotalToPay,
                Detail = item.Object.Detail,
                Date = item.Object.Date,
                PayFormat = item.Object.PayFormat,
                ShoppingCar = item.Object.ShoppingCar,
                State = item.Object.State
            }).Where(h => h.ClientMail.ToLower().Contains(Preferences.Get("Email", "").ToLower())).ToList();
        }
        //==============================================================================

        //==============================================================================
        public static async Task<List<Ventas>> GetPendingSales()
        {
            string aux = "Pendiente";
            return (await firebase.Child(nameof(Ventas)).OnceAsync<Ventas>()).Select(item => new Ventas
            {
                Id = item.Key,
                ClientName = item.Object.ClientName,
                ClientMail = item.Object.ClientMail,
                ClientPhone = item.Object.ClientPhone,
                ClientLatitude = item.Object.ClientLatitude,
                ClientLongitude = item.Object.ClientLongitude,
                TotalToPay = item.Object.TotalToPay,
                Detail = item.Object.Detail,
                Date = item.Object.Date,
                PayFormat = item.Object.PayFormat,
                ShoppingCar = item.Object.ShoppingCar,
                State = item.Object.State
            }).Where(h => h.State.ToLower().Contains(aux.ToLower())).ToList();
        }
        //==============================================================================

        //==============================================================================
        public static async Task<bool> UpdateState(Ventas sale)
        {
            await firebase.Child(nameof(Ventas) + "/" + sale.Id).PutAsync(JsonConvert.SerializeObject(sale));
            return true;
        }
    }
}