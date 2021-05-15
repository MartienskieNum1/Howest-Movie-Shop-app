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
    [Route("[controller]")]
    public class DetailsController : Controller
    {
        MovieService movieService = new MovieService();
        GenreMovieService genreMovieService = new GenreMovieService();
        GenreService genreService = new GenreService();
        MovieRoleService movieRoleService = new MovieRoleService();
        PersonService personService = new PersonService();
        SessionService sessionService = new SessionService();

        [Route("/Details")]
        [AllowAnonymous]
        public IActionResult MovieDetail(int id)
        {
            var movie = movieService.GetMovieForMovieId(id);
            int genreId = genreMovieService.GetGenreIdForMovieId(id);
            string genre = genreService.GetGenreForGenreId(genreId);
            List<int> personIds = movieRoleService.GetPersonIdsForMovieIdAndRole(id, "actor");
            List<string> personNames = personService.GetPersonNamesForPersonIds(personIds);
            
            return View("~/Views/Movie/MovieDetail.cshtml", new MovieViewModel
            {
                Id = Convert.ToInt32(movie.Id),
                Title = movie.Title,
                Url = movie.CoverUrl,
                Year = movie.Year,
                Rating = movie.Rating,
                Genre = genre,
                Plot = movie.Plot,
                Actors = personNames,
                CartAmount = sessionService.GetCartAmount(HttpContext.Session)
            });
        }

        [HttpPost]
        [Route("[action]")]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public IActionResult Order(MovieViewModel model)
        {
            sessionService.AddToCart(HttpContext.Session, model.Id);
            return RedirectToAction("Movies", "Movie");
        }
    }
}