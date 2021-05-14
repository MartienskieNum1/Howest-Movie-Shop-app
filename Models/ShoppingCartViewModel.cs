using System;
using System.Collections.Generic;

namespace app.Models
{
    public class ShoppingCartViewModel
    {
        public List<MovieViewModel> Movies {get; set;}
        public int CartAmount {get; set;}
    }
}