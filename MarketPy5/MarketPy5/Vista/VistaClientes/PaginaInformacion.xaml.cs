using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MarketPy5.Vista.VistaClientes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaInformacion : ContentPage
    {
        public PaginaInformacion()
        {
            InitializeComponent();
        }

        public async Task SendEmail()
        {
            List<string> recipients = new List<string>();
            recipients.Add("antoniza.developer@gmail.com");

            try
            {
                var message = new EmailMessage
                {
                    Subject = "Notificación de error en aplicación.",
                    Body = "Saludos Cordiales al departamento de IT.\n\n Por el presente mes gustaria reportar un error en la aplicación del supermercado El Económico.\n\nEl error consiste en ...",
                    To = recipients,
                    //Cc = Preferences.Get("Email", ""),
                    //Bcc = bccRecipients
                };
                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException fbsEx)
            {
                // Email is not supported on this device
            }
            catch (Exception ex)
            {
                // Some other exception occurred
            }
        }

        private async void SendErrorMail_Clicked(object sender, EventArgs e)
        {
            await SendEmail();
        }
    }
}