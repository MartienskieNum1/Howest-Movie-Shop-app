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
    public class MovieController : Controller
    {
        MovieService movieService = new MovieService();
        ShopMoviePriceService shopMoviePriceService = new ShopMoviePriceService();

        public IActionResult Movies()
        {
            var movies = movieService.All();

            return View(new MoviesViewModel
            {
                Count = movies.Count(),
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

        [HttpPost]
        [Route("/Movie")]
        public IActionResult Filter(MoviesViewModel model)
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
                            movies = movieService.All()
                                .OrderBy(m => m.Title);
                            break;
                        default:
                            movies = movieService.All()
                                .OrderBy(m => m.Year);
                            break;
                    }
                } else
                {
                    switch (sortKey)
                    {
                        case "Title":
                            movies = movieService.All()
                                .OrderByDescending(m => m.Title);
                            break;
                        default:
                            movies = movieService.All()
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
                            movies = movieService.All()
                                .Where(m => m.Title.Contains(model.SearchValue))
                                .OrderBy(m => m.Title);
                            break;
                        default:
                            movies = movieService.All()
                                .Where(m => m.Title.Contains(model.SearchValue))
                                .OrderBy(m => m.Year);
                            break;
                    }
                } else
                {
                    switch (sortKey)
                    {
                        case "Title":
                            movies = movieService.All()
                                .Where(m => m.Title.Contains(model.SearchValue))
                                .OrderByDescending(m => m.Title);
                            break;
                        default:
                            movies = movieService.All()
                                .Where(m => m.Title.Contains(model.SearchValue))
                                .OrderByDescending(m => m.Year);
                            break;
                    }
                }
            }
            

            return View("Movies", new MoviesViewModel
            {
                Count = movies.Count(),
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