using System;
using System.Collections.Generic;
using System.Text;

namespace MarketPy5.Modelo
{
    public class Ventas
    {
        public String Id { get; set; }
        public String ClientName { get; set; }
        public String ClientPhone { get; set; }
        public String ClientMail { get; set; }
        public String ClientLatitude { get; set; }
        public String ClientLongitude { get; set; }
        public String Detail { get; set; }
        public String Date { get; set; }
        public String PayFormat { get; set; }
        public String TotalToPay { get; set; }
        public String ShoppingCar { get; set; }
        public String State { get; set; }
    }
}
