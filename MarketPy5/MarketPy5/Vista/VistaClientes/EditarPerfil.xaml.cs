using MarketPy5.Modelo;
using MarketPy5.Controlador;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MarketPy5.Vista.VistaClientes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditarPerfil : ContentPage
    {
        MediaFile file;
        bool ImageEdited;
        CancellationTokenSource cts;

        [Obsolete]
        public EditarPerfil()
        {
            InitializeComponent();
            FillForm();

            UserImage.GestureRecognizers.Add(new TapGestureRecognizer((view) => ChooesImage()));
            GetLocationButton.GestureRecognizers.Add(new TapGestureRecognizer((view) => OpenModal()));
            ClosePopUpModal.GestureRecognizers.Add(new TapGestureRecognizer((view) => CloseModal()));

            CloseModal();
        }

        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
        private void OpenModal()
        {
            GetCurrentLocation();
            PopUpModal.IsVisible = true;
        }

        private void CloseModal()
        {
            PopUpModal.IsVisible = false;
        }

        private void FillForm()
        {
            UserImage.Source = Preferences.Get("Image", "https://i.ibb.co/vhh0Gkj/users.png");
            UserNameText.Text = Preferences.Get("Username", "NoUserNameFounded");
            UserPhoneText.Text = Preferences.Get("Phone", "NoUserPhoneFounded");
            UserEmailText.Text = Preferences.Get("Email", "NoUserEmailFounded");
            GenderPicker.Title = Preferences.Get("Gender", "Seleccione su genero");
            UserAgeText.Text = Preferences.Get("Age", "");

            if (Preferences.Get("Latitude", "") == "" || Preferences.Get("Longitude", "") == "")
            {
                GetCurrentLocationAndApply();
            }
            else
            {
                LoadLocation();
            }
        }
        async Task GetCurrentLocationAndApply()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cts.Token);

                if (location != null)
                {
                    var placemarks = await Geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude);
                    var placemark = placemarks?.FirstOrDefault();

                    UserLocationText.Text = $" ({placemark.CountryCode}) {placemark.CountryName}, {placemark.AdminArea}, {placemark.Locality}";
                    LatitudeCord.Text = $"{location.Latitude}";
                    LongitudeCord.Text = $"{location.Longitude}";
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }

        async Task LoadLocation()
        {
            try
            {
                double latitude = Convert.ToDouble(Preferences.Get("Latitude", ""));
                double longitude = Convert.ToDouble(Preferences.Get("Longitude", ""));

                var placemarks = await Geocoding.GetPlacemarksAsync(latitude, longitude);
                var placemark = placemarks?.FirstOrDefault();

                UserLocationText.Text = $" ({placemark.CountryCode}) {placemark.CountryName}, {placemark.AdminArea}, {placemark.Locality}";
                LatitudeCord.Text = Preferences.Get("Latitude", "");
                LongitudeCord.Text = Preferences.Get("Longitude", "");
            }
            catch (Exception ex)
            {
                await DisplayAlert("No ubicacionb", ex.Message, "OK");
            }
        }

        async Task GetCurrentLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cts.Token);

                if (location != null)
                {
                    var placemarks = await Geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude);
                    var placemark = placemarks?.FirstOrDefault();

                    NewLocation.Text = $"({placemark.CountryCode}) {placemark.CountryName}\n " +
                        $"{placemark.AdminArea}\n " +
                        $"{placemark.Locality}\n " +
                        $"{location.Latitude}, {location.Longitude}";
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }

        protected override void OnDisappearing()
        {
            if (cts != null && !cts.IsCancellationRequested)
                cts.Cancel();
            base.OnDisappearing();
        }

        private async void ChooesImage()
        {
            string action = await DisplayActionSheet("Opciones:", "Cancel", null, "Tomar Foto", "Seleccionar Foto");
            if (action == "Seleccionar Foto")
            {
                SelectImage();
            }
            else if (action == "Tomar Foto")
            {
                MakeImage();
            }
        }

        private async void SelectImage()
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
                UserImage.Source = ImageSource.FromStream(() =>
                {
                    ImageEdited = true;
                    return file.GetStream();
                });
            }
            catch (Exception ex)
            {

            }
        }

        private async void MakeImage()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No permiso", "camara inalcanzable", "OK");
                return;
            }

            try
            {
                file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "Profiles",
                    Name = "profiole.jpg",
                    SaveToAlbum = true,
                    CompressionQuality = 75,
                    CustomPhotoSize = 50,
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.MaxWidthHeight,
                    MaxWidthHeight = 2000,
                    DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Front
                });

                if (file == null)
                {
                    return;
                }

                UserImage.Source = ImageSource.FromStream(() =>
                {
                    ImageEdited = true;
                    return file.GetStream();
                });
            }
            catch (Exception e)
            {
                await DisplayAlert("Error", e.Message, "OK");
            }
        }

        private async void NewLocationButton_Clicked(object sender, EventArgs e)
        {
            var res = await DisplayAlert("Confirmar", "¿Quiere establecer esta ubicacion como direccion predeterminada?", "Si", "Cancelar");

            if (res)
            {
                await GetCurrentLocationAndApply();
                CloseModal();
            }
            else
            {
                CloseModal();
            }
        }

        private async void UpdateButton_Clicked(object sender, EventArgs e)
        {
            string Gender = string.Empty;
            string NewUserImage;

            if (string.IsNullOrEmpty(UserNameText.Text) || string.IsNullOrEmpty(UserEmailText.Text) || string.IsNullOrEmpty(UserPhoneText.Text))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Información necesaria incompleta", "OK");
            }

            if (ImageEdited)
            {
                NewUserImage = await UserControlador.SaveImage(file.GetStream(), Path.GetFileName(file.Path));
            }
            else
            {
                NewUserImage = Preferences.Get("Image", "https://i.ibb.co/vhh0Gkj/users.png");
            }

            if (GenderPicker.SelectedIndex != null && GenderPicker.SelectedIndex >= 0)
            {
                Gender = GenderPicker.Items[GenderPicker.SelectedIndex];
            }
            else
            {
                Gender = Preferences.Get("Gender", "");
            }

            Usuarios dataUpdate = new Usuarios()
            {
                Id = Preferences.Get("Id", ""),
                Name = UserNameText.Text,
                Email = UserEmailText.Text,
                Image = NewUserImage,
                Level = "C",
                Latitude = LatitudeCord.Text,
                Longitude = LongitudeCord.Text,
                Phone = UserPhoneText.Text,
                Age = UserAgeText.Text,
                Gender = Gender
            };
            Preferences.Set("Username", UserNameText.Text);
            Preferences.Set("Email", UserEmailText.Text);
            Preferences.Set("Phone", UserPhoneText.Text);
            Preferences.Set("Image", NewUserImage);
            Preferences.Set("Gender", Gender);
            Preferences.Set("Latitude", LatitudeCord.Text);
            Preferences.Set("Longitude", LongitudeCord.Text);
            Preferences.Set("Age", UserAgeText.Text);

            bool res = await UserControlador.UpdateUser(dataUpdate);

            if (res)
            {
                await Application.Current.MainPage.DisplayAlert("Satisfactorio", "Perfil actualizado satisfactoriamente.", "OK");

                await Navigation.PushAsync(new PaginaInicio());
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Fallo de sistema", "No se pudo actualizar el cliente.", "OK");
            }
        }
    }
}