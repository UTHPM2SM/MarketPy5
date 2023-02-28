using MarketPy5.ModelosVista;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MarketPy5.Vista
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Registros : ContentPage
	{
        [Obsolete]
        public Registros ()
		{
			InitializeComponent ();
            GoToLogin.GestureRecognizers.Add(new TapGestureRecognizer((view) => ToLogin()));

            BindingContext = new ModeloVistaRegistro();
        }

        void ToLogin()
        {
            Navigation.PopAsync();
        }

    }
}