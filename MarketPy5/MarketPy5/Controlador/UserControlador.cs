using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using Newtonsoft.Json;
using System.IO;
using Xamarin.Essentials;
using Firebase.Auth;
using MarketPy5.Vista;

namespace MarketPy5.Controlador
{
    class UserControlador
    {

        private string WebApiKey = "AIzaSyA33uUzHinDOOitFq-WNed3dlctMJJmjyk";
        public static FirebaseClient firebase = new FirebaseClient("https://proyectosupermercado-728d4-default-rtdb.firebaseio.com/");
        public static FirebaseStorage firebaseStorage = new FirebaseStorage("proyectosupermercado-728d4.appspot.com");
        /**/
        //Lista de usuarios: Atrae todos los usuarios
        public static async Task<List<Usuarios>> GetAllUser()
        {
            try
            {
                var userlist = (await firebase
                .Child("Users")
                .OnceAsync<Usuarios>()).Select(item =>
                new Usuarios
                {
                    Id = item.Key,
                    Name = item.Object.Name,
                    Email = item.Object.Email,
                    Phone = item.Object.Phone,
                    Image = item.Object.Image,
                    Level = item.Object.Level,
                    Latitude = item.Object.Latitude,
                    Longitude = item.Object.Longitude,
                    Gender = item.Object.Gender,
                    Age = item.Object.Age
                }).ToList();
                return userlist;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return null;
            }
        }

        //Añade a un usuario
        public static async Task<bool> AddUser(string name, string email, string phone)
        {
            try
            {
                await firebase
                .Child("Users")
                .PostAsync(new Usuarios() { Name = name, Email = email, Phone = phone, Level = "C", Image = "https://i.ibb.co/vhh0Gkj/users.png", Gender = "", Latitude = "", Longitude = "", Age = "" });
                return true;
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Alerta", e.Message, "OK");
                return false;
            }
        }

        //Extrae a un usuario segun su correo

        public static async Task<Usuarios> GetUser(string email)
        {
            try
            {
                var allUsers = await GetAllUser();
                await firebase
                .Child("Users")
                .OnceAsync<Usuarios>();
                return allUsers.Where(a => a.Email == email).FirstOrDefault();

            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error:{e}");
                return null;
            }
        }

        //Verificacion del email
        public async void EmailVerification(string email)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebApiKey));
            await authProvider.SendEmailVerificationAsync(email);

            await Application.Current.MainPage.DisplayAlert("Verificación", "Se ha enviado a " + email + " un correo de verificación", "OK");
        }

        //Actualizar usuario
        public static async Task<bool> UpdateUser(Usuarios user)
        {
            await firebase.Child(nameof(Usuarios) + "/" + user.Id).PutAsync(JsonConvert.SerializeObject(user));
            return true;
        }

        //Login del usuario, con parametros de correo y contraseña
        public async void Login(string email, string password)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebApiKey));
            try
            {
                var auth = await authProvider.SignInWithEmailAndPasswordAsync(email, password);
                var content = await auth.GetFreshAuthAsync();
                var serializedcontnet = JsonConvert.SerializeObject(content);
                Preferences.Set("MyFirebaseRefreshToken", serializedcontnet);
                var user = await GetUser(email);
                Preferences.Set("Id", user.Id);
                Preferences.Set("Username", user.Name);
                Preferences.Set("Email", user.Email);
                Preferences.Set("Phone", user.Phone);
                Preferences.Set("Level", user.Level);
                Preferences.Set("Image", user.Image);
                Preferences.Set("Gender", user.Gender);
                Preferences.Set("Latitude", user.Latitude);
                Preferences.Set("Longitude", user.Longitude);
                Preferences.Set("Age", user.Age);

                if (user.Level == "C")
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new Cliente());
                }
                else if (user.Level == "D")
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new Repartidor());
                }
                else if (user.Level == "A")
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new Administrador());
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error de usuario", "No se le pudo asignar nivel a esta cuenta", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Fallo de sesión", "Correo o contraseña incorrectas.", "OK");
            }
        }

        //Registrador de usuarios

        public async void Register(string name, string email, string phone, string password)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(password))
            {
                await Application.Current.MainPage.DisplayAlert("Alerta", "Se requiere llenar todos los campos del formulario.", "OK");
            }
            else
            {
                if (password.Length < 8)
                {
                    await Application.Current.MainPage.DisplayAlert("Advertencia", "La contraseña debe tener 8 caracteres minimo.", "OK");
                }
                else
                {
                    try
                    {
                        var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebApiKey));
                        var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(email, password);
                        string gettoken = auth.FirebaseToken;
                        await AddUser(name, email, phone);
                        Login(email, password);
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("EMAIL_EXIST"))
                        {
                            await Application.Current.MainPage.DisplayAlert("Error de cuenta", "Esta cuenta ya existe", "OK");
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Alert", ex.Message, "OK");
                        }
                    }
                }
            }
        }

        //Guardado de iamgenes
        public static async Task<string> SaveImage(Stream image, string filename)
        {
            var img = await firebaseStorage.Child("UsersImages").Child(filename).PutAsync(image);
            return img;
        }

        //Salida de sesión
        public void LogOut()
        {
            Preferences.Remove("MyFirebaseRefreshToken");
            Preferences.Remove("Username");
            Preferences.Remove("Email");
            Preferences.Remove("Image");
            Preferences.Remove("Level");
            Preferences.Remove("Phone");
            Preferences.Remove("Gender");
            Preferences.Remove("Latitude");
            Preferences.Remove("Longitude");
            Preferences.Remove("Age");
            Application.Current.MainPage = new NavigationPage(new MainPage());
        }


    }
}
