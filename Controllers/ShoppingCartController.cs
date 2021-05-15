using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using app.Models;
using app.Services;
using lib.Library.Services;

namespace app.Controllers
{
    [Route("[controller]")]
    public class ShoppingCartController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        ShopCustomerService shopCustomerService = new ShopCustomerService();
        ShopOrderService shopOrderService = new ShopOrderService();
        ShopOrderDetailService shopOrderDetailService = new ShopOrderDetailService();
        SessionService sessionService = new SessionService();

        public ShoppingCartController(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }

        [Route("/ShoppingCart")]
        [AllowAnonymous]
        public IActionResult ShoppingCart()
        {
            List<int> ids = sessionService.GetCart(HttpContext.Session);

            List<MovieViewModel> movies = new List<MovieViewModel>();
            if (ids != null)
            {
                movies = sessionService.GetMoviesForMovieIds(HttpContext.Session, ids)
                            .Select(m => {
                                var price = sessionService.GetPriceForMovieId(HttpContext.Session, Convert.ToInt32(m.Id));
                                return new MovieViewModel
                                {
                                    Title = m.Title,
                                    Price = price
                                };
                            })
                            .ToList();
            }

            sessionService.SetInOrderProcess(HttpContext.Session, true);

            return View("~/Views/Movie/Shoppingcart.cshtml", new ShoppingCartViewModel
            {
                CartAmount = sessionService.GetCartAmount(HttpContext.Session),
                Movies = movies
            });
        }

        [Route("[action]")]
        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            var user = await userManager.GetUserAsync(HttpContext.User);

            return View("~/Views/Movie/Checkout.cshtml", new CheckoutViewModel
            {
                Name = user.UserName
            });
        }

        [Route("[action]")]
        [Authorize]
        public async Task<IActionResult> Confirm(CheckoutViewModel model)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);

            int customerId;
            if (shopCustomerService.CustomerExistsForUserId(user.Id))
            {
                customerId = shopCustomerService.GetCustomerIdForUserId(user.Id);
            } else
            {
                customerId = shopCustomerService.Add(user.Id, model.Name, model.Street, model.City, model.PostalCode, model.Country);
            }

            int orderId = shopOrderService.Add(customerId, model.Street, model.City, model.PostalCode, model.Country);
            foreach (int movieId in sessionService.GetCart(HttpContext.Session))
            {
                decimal price = sessionService.GetPriceForMovieId(HttpContext.Session, movieId);
                shopOrderDetailService.Add(orderId, movieId, price);
            }

            sessionService.ClearCart(HttpContext.Session);
            sessionService.SetInOrderProcess(HttpContext.Session, false);

            return View("~/Views/Movie/Confirmation.cshtml", new CheckoutViewModel
            {
                Name = model.Name,
                PaymentMethod = model.PaymentMethod.Replace("_", " ")
            });
        }
    }
}