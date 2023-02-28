using GalaSoft.MvvmLight.Command;
using MarketPy5.Controlador;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace MarketPy5.ModelosVista
{
    class ModeloVistaRegistro : ModeloVistaBase
    {
        private string name;
        private string email;
        private string phone;
        private string password;

        //==============================================================================

        //==============================================================================
        
        public string RegisterNameText
        {
            get { return this.name; }
            set { SetValue(ref this.name, value); }
        }
        public string RegisterEmailText
        {
            get { return this.email; }
            set { SetValue(ref this.email, value); }
        }
        public string RegisterPhoneText
        {
            get { return this.phone; }
            set { SetValue(ref this.phone, value); }
        }
        public string RegisterPasswordText
        {
            get { return this.password; }
            set { SetValue(ref this.password, value); }
        }
      
        //==============================================================================

       

        //==============================================================================
        
        public ICommand RegisterCommand
        {
            get
            {
                return new RelayCommand(Register);
            }
        }
       

        
        private async void Register()
        {
            UserControlador obj = new UserControlador();
            obj.Register(name, email, phone, password);
        }


    }
}
