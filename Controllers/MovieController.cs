using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using lib.Library.Services;
using app.Models;
using app.Services;

namespace app.Controllers
{
    [Route("")]
    [Route("[controller]")]
    public class MovieController : Controller
    {
        MovieService movieService = new MovieService();
        ShopMoviePriceService shopMoviePriceService = new ShopMoviePriceService();
        SessionService sessionService = new SessionService();

        [Route("")]
        [Route("[action]")]
        [AllowAnonymous]
        public IActionResult CheckProcess()
        {
            bool inProcess = sessionService.GetInOrderProcess(HttpContext.Session);
            if (inProcess)
            {
                return RedirectToAction("ShoppingCart", "ShoppingCart");
            } else
            {
                return RedirectToAction("Movies");
            }
        }

        [Route("[action]")]
        [AllowAnonymous]
        public IActionResult Movies()
        {
            var movies = sessionService.GetMovies(HttpContext.Session);
            if (movies == null)
            {
                movies = movieService.All();
                sessionService.StoreMovies(HttpContext.Session, movies);
                sessionService.StorePrices(HttpContext.Session, shopMoviePriceService.All());
            }

            sessionService.SetInOrderProcess(HttpContext.Session, false);

            MoviesViewModel filter = sessionService.GetFilter(HttpContext.Session);
            if (filter != null)
            {
                return Filter(filter);
            }

            return View(new MoviesViewModel
            {
                Count = movies.Count(),
                CartAmount = sessionService.GetCartAmount(HttpContext.Session),
                Movies = movies.Select(m => {
                    var price = sessionService.GetPriceForMovieId(HttpContext.Session, Convert.ToInt32(m.Id));
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

        [HttpPost]
        [Route("[action]")]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public IActionResult Order(MoviesViewModel model)
        {
            sessionService.AddToCart(HttpContext.Session, model.MovieId);
            return RedirectToAction("Movies");
        }

        [HttpPost]
        [Route("/Movie")]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public IActionResult Filter(MoviesViewModel model)
        {
            sessionService.SetFilter(HttpContext.Session, model);

            var movies = GetFilteredMovies(model);

            return View("Movies", new MoviesViewModel
            {
                SearchValue = model.SearchValue,
                SortKey = model.SortKey,
                SortOrder = model.SortOrder,
                Count = movies.Count(),
                CartAmount = sessionService.GetCartAmount(HttpContext.Session),
                Movies = movies.Select(m => {
                    var price = sessionService.GetPriceForMovieId(HttpContext.Session, Convert.ToInt32(m.Id));
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

        private System.Linq.IOrderedEnumerable<lib.Library.Models.Movie> GetFilteredMovies(MoviesViewModel model)
        {
            System.Linq.IOrderedEnumerable<lib.Library.Models.Movie> movies;
            string sortKey = model.SortKey;
            string sortOrder = model.SortOrder;

            if (model.SearchValue == null)
            {
                if (sortOrder == "asc")
                {
                    switch (sortKey)
                    {
                        case "Title":
                            movies = sessionService.GetMovies(HttpContext.Session)
                                .OrderBy(m => m.Title);
                            break;
                        default:
                            movies = sessionService.GetMovies(HttpContext.Session)
                                .OrderBy(m => m.Year);
                            break;
                    }
                } else
                {
                    switch (sortKey)
                    {
                        case "Title":
                            movies = sessionService.GetMovies(HttpContext.Session)
                                .OrderByDescending(m => m.Title);
                            break;
                        default:
                            movies = sessionService.GetMovies(HttpContext.Session)
                                .OrderByDescending(m => m.Year);
                            break;
                    }
                }
            } else
            {
                if (sortOrder == "asc")
                {
                    switch (sortKey)
                    {
                        case "Title":
                            movies = sessionService.GetMovies(HttpContext.Session)
                                .Where(m => m.Title.ToLower().Contains(model.SearchValue.ToLower()))
                                .OrderBy(m => m.Title);
                            break;
                        default:
                            movies = sessionService.GetMovies(HttpContext.Session)
                                .Where(m => m.Title.ToLower().Contains(model.SearchValue.ToLower()))
                                .OrderBy(m => m.Year);
                            break;
                    }
                } else
                {
                    switch (sortKey)
                    {
                        case "Title":
                            movies = sessionService.GetMovies(HttpContext.Session)
                                .Where(m => m.Title.ToLower().Contains(model.SearchValue.ToLower()))
                                .OrderByDescending(m => m.Title);
                            break;
                        default:
                            movies = sessionService.GetMovies(HttpContext.Session)
                                .Where(m => m.Title.ToLower().Contains(model.SearchValue.ToLower()))
                                .OrderByDescending(m => m.Year);
                            break;
                    }
                }
            }

            return movies;
        }
    }
}