using System;
using System.Collections.Generic;

namespace app.Models
{
    public class MoviesViewModel
    {
        public int Count {get; set;}
        public List<MovieViewModel> Movies {get; set;}

        public string SearchValue {get; set;}
        public string SortKey {get; set;}
        public string SortOrder {get; set;}
        
        public int MovieId {get; set;}
        public int CartAmount {get; set;}
    }
}