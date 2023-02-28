using Firebase.Auth;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MarketPy5.ModelosVista
{
    class ModeloVistaContrasenia : ModeloVistaBase
    {

       
        private string email;

        private string WebApiKey = "AIzaSyA33uUzHinDOOitFq-WNed3dlctMJJmjyk";
        //==============================================================================

        //==============================================================================
        
        public string PasswordEmailText
        {
            get { return this.email; }
            set { SetValue(ref this.email, value); }
        }
        //==============================================================================

        //==============================================================================
        public ICommand RecoveryCommand
        {
            get
            {
                return new RelayCommand(SendMail);
            }
        }
        //==============================================================================

        //==============================================================================
        public async void SendMail()
        {
            if (string.IsNullOrEmpty(email))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Tienes que llenar los campos del formulario.", "OK");
            }
            else
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebApiKey));

                try
                {
                    await authProvider.SendPasswordResetEmailAsync(email);
                    await Application.Current.MainPage.DisplayAlert("Enviado", "Se le ha enviado un correo para restablecer su contraseña. Si no lo encuentra, revise la carpeta de Spam", "OK");
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
                catch (Exception e)
                {
                }
            }
        }


    }
}
