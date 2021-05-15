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
                    var movies = sessionService.GetMoviesForMovieIds(HttpContext.Session, movieIds);
                    Dictionary<string, int> movieDict = new Dictionary<string, int>();
                    decimal totalPrice = 0;
                    foreach (var movie in movies)
                    {
                        movieDict.Add(movie.Title, Convert.ToInt32(movie.Id));
                        totalPrice += shopOrderDetailService.GetPriceForMovieIdAndOrderId(Convert.ToInt32(movie.Id), Convert.ToInt32(o.Id));
                    }

                    return new OrderViewModel
                    {
                        OrderId = Convert.ToInt32(o.Id),
                        OrderDate = ((DateTime) o.OrderDate).ToString("d", CultureInfo.CreateSpecificCulture("fr-FR")),
                        Address = $"{o.Street}, {o.PostalCode} {o.City}, {o.Country}",
                        CustomerName = customer.Name,
                        Movies = movieDict,
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