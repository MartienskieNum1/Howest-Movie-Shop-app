using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using app.Models;
using app.Services;
using lib.Library.Services;

namespace app.Controllers
{
    [Route("[controller]")]
    public class ShoppingCartController : Controller
    {
        MovieService movieService = new MovieService();
        ShopMoviePriceService shopMoviePriceService = new ShopMoviePriceService();
        SessionService sessionService = new SessionService();

        [Route("/ShoppingCart")]
        public IActionResult ShoppingCart()
        {
            List<int> ids = sessionService.GetCart(HttpContext.Session);
            var movies = movieService.GetMoviesForMovieIds(ids);

            return View("~/Views/Movie/Shoppingcart.cshtml", new ShoppingCartViewModel
            {
                Movies = movies.Select(m => {
                    var price = shopMoviePriceService.GetPriceForMovieId(Convert.ToInt32(m.Id));
                    return new MovieViewModel
                    {
                        Title = m.Title,
                        Price = price
                    };
                })
                .ToList()
            });
        }
    }
}