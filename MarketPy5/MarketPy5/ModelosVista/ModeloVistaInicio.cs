using GalaSoft.MvvmLight.Command;
using MarketPy5.Controlador;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace MarketPy5.ModelosVista
{
    class ModeloVistaInicio
    {
        #region Attributes
        private string userName;

        private string WebApiKey = "AIzaSyA33uUzHinDOOitFq-WNed3dlctMJJmjyk";
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
