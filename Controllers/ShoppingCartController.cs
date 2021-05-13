using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using app.Models;
using app.Services;
using lib.Library.Services;

namespace app.Controllers
{
    [Route("[controller]")]
    public class ShoppingCartController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        MovieService movieService = new MovieService();
        ShopMoviePriceService shopMoviePriceService = new ShopMoviePriceService();
        SessionService sessionService = new SessionService();

        public ShoppingCartController(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }

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

        [Route("[action]")]
        public async Task<IActionResult> Checkout()
        {
            var user = await userManager.GetUserAsync(HttpContext.User);

            return View("~/Views/Movie/Checkout.cshtml", new CheckoutViewModel
            {
                Name = user.UserName
            });
        }
    }
}