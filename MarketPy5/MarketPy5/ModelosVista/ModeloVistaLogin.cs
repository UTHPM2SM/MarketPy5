using GalaSoft.MvvmLight.Command;
using MarketPy5.Controlador;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace MarketPy5.ModelosVista
{
    class ModeloVistaLogin : ModeloVistaBase
    {

       
        private string email;
        private string password;
        
        //==============================================================================

        //==============================================================================
       
        public string LoginEmailText
        {
            get { return this.email; }
            set { SetValue(ref this.email, value); }
        }
        public string LoginPasswordText
        {
            get { return this.password; }
            set { SetValue(ref this.password, value); }
        }
       
        //==============================================================================

        //==============================================================================
        
        public ICommand LoginCommand
        {
            get
            {
                return new RelayCommand(Login);
            }
        }
        
        //==============================================================================

        //==============================================================================
       
        private async void Login()
        {
            UserControlador obj = new UserControlador();
            obj.Login(email, password);
        }
       
    }
}
