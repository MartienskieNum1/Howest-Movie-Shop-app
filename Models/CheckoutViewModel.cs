using System;
using System.Collections.Generic;

namespace app.Models
{
    public class CheckoutViewModel
    {
        public string Name {get; set;}
        public string Street {get; set;}

        public string City {get; set;}
        public string PostalCode {get; set;}
        public string Country {get; set;}
        
        public string PaymentMethod {get; set;}
    }
}