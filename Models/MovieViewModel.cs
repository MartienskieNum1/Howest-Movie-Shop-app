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
        public decimal Rating {get; set;}
        public string Genre {get; set;}
        public string Plot {get; set;}
        public List<string> Actors {get; set;}
        public int CartAmount {get; set;}
    }
}