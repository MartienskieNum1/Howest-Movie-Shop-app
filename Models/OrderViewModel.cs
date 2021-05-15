using System;
using System.Collections.Generic;

namespace app.Models
{
    public class OrderViewModel
    {
        public int OrderId {get; set;}
        public string OrderDate {get; set;}
        public Dictionary<string, int> Movies {get; set;}
        public decimal TotalPrice {get; set;}
        public string CustomerName {get; set;}
        public string Address {get; set;}
    }
}