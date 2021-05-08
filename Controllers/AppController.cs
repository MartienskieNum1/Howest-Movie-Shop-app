using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using lib.Library.Services;
using app.Models;

namespace app.Controllers
{
    [Route("")]
    [Route("[controller]")]
    public class AppController : Controller
    {
        MovieService movieService = new MovieService();
        ShopMoviePriceService shopMoviePriceService = new ShopMoviePriceService();

        public IActionResult Movies()
        {
            var movies = movieService.All();

            return View(new MoviesViewModel
            {
                Movies = movies.Select(m => {
                    var price = shopMoviePriceService.GetPriceForMovieId(Convert.ToInt32(m.Id));
                    return new MovieViewModel
                    {
                        Id = Convert.ToInt32(m.Id),
                        Title = m.Title,
                        Url = m.CoverUrl,
                        Year = m.Year,
                        Price = price
                    };
                })
                .ToList()
            });
        }
    }
}