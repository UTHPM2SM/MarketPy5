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
	public partial class RecuperacionContra : ContentPage
	{
        [Obsolete]
        public RecuperacionContra ()
		{
			InitializeComponent();
            BindingContext = new ModeloVistaContrasenia();

            GoToLogin.GestureRecognizers.Add(new TapGestureRecognizer((view) => ToLogin()));
        }


        void ToLogin()
        {
            Navigation.PopAsync();
        }

    }
}