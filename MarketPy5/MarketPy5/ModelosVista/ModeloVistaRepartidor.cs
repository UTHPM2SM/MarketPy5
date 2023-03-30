using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using MarketPy5.Controlador;

namespace MarketPy5.ModelosVista
{
     class ModeloVistaRepartidor
    {
        #region Attributes
        private string userName;

        #endregion
        //==============================================================================

        //==============================================================================
        #region Properties
        public string UserName
        {
            get { return this.userName; }
        }
        #endregion
        //==============================================================================

        //==============================================================================
        #region Commands
        public ICommand LogOutCommand
        {
            get
            {
                return new RelayCommand(LogOut);
            }
        }
        #endregion
        //==============================================================================

        //==============================================================================
        #region Methods

        public void LogOut()
        {
            UserControlador obj = new UserControlador();
            obj.LogOut();
        }
        #endregion
    }
}