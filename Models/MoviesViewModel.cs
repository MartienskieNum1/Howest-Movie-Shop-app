using System;
using System.Collections.Generic;

namespace app.Models
{
    public class MoviesViewModel
    {
        public int Count {get; set;}
        public List<MovieViewModel> Movies {get; set;}
    }
}