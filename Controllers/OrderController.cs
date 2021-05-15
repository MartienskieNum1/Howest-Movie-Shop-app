using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using app.Models;
using lib.Library.Services;
using app.Services;

namespace app.Controllers
{
    [Route("[controller]")]
    public class OrderController : Controller
    {
        ShopOrderService shopOrderService = new ShopOrderService();
        ShopCustomerService shopCustomerService = new ShopCustomerService();
        ShopOrderDetailService shopOrderDetailService = new ShopOrderDetailService();
        MovieService movieService = new MovieService();
        SessionService sessionService = new SessionService();

        [Route("/Order")]
        [Authorize(Roles = "Admin")]
        public IActionResult Orders()
        {
            var orders = shopOrderService.All();

            return View("~/Views/Movie/Orders.cshtml", new OrdersViewModel
            {
                CartAmount = sessionService.GetCartAmount(HttpContext.Session),
                Orders = orders.Select(o => {
                    var customer = shopCustomerService.GetCustomerForCustomerId(Convert.ToInt32(o.CustomerId));
                    List<int> movieIds = shopOrderDetailService.GetMovieIdsForOrderId(Convert.ToInt32(o.Id));
                    var movies = movieService.GetMoviesForMovieIds(movieIds);
                    List<string> movieTitles = new List<string>();
                    decimal totalPrice = 0;
                    foreach (var movie in movies)
                    {
                        movieTitles.Add(movie.Title);
                        totalPrice += shopOrderDetailService.GetPriceForMovieIdAndOrderId(Convert.ToInt32(movie.Id), Convert.ToInt32(o.Id));
                    }

                    return new OrderViewModel
                    {
                        OrderId = Convert.ToInt32(o.Id),
                        OrderDate = ((DateTime) o.OrderDate).ToString("d", CultureInfo.CreateSpecificCulture("fr-FR")),
                        Address = $"{o.Street}, {o.PostalCode} {o.City}, {o.Country}",
                        CustomerName = customer.Name,
                        Titles = movieTitles,
                        TotalPrice = totalPrice
                    };
                })
                .ToList()
            });
        }

        [Route("/Delete")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            shopOrderService.Delete(id);
            shopOrderDetailService.DeleteOrderDetailsForOrderId(id);

            return RedirectToAction("Orders");
        }
    }
}