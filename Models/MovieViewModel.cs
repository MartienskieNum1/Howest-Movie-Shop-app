using System;
using System.Collections.Generic;

namespace app.Models
{
    public class MovieViewModel
    {
        public int Id {get; set;}
        public string Title {get; set;}
        public string Url {get; set;}
        public int Year {get; set;}
        public decimal Price {get; set;}
    }
}