using System;
using System.Collections.Generic;

namespace app.Models
{
    public class OrdersViewModel
    {
        public List<OrderViewModel> Orders {get; set;}
        public int CartAmount {get; set;}
    }
}