using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using lib.Library.Services;
using app.Models;

namespace app.Controllers
{
    public class DetailsController : Controller
    {
        MovieService movieService = new MovieService();
        GenreMovieService genreMovieService = new GenreMovieService();
        GenreService genreService = new GenreService();

        [Route("/Details")]
        public IActionResult MovieDetail(int id)
        {
            var movie = movieService.GetMovieForMovieId(id);
            int genreId = genreMovieService.GetGenreIdForMovieId(id);
            string genre = genreService.GetGenreForGenreId(genreId);
            return View("~/Views/Movie/MovieDetail.cshtml", new MovieViewModel
            {
                Id = Convert.ToInt32(movie.Id),
                Title = movie.Title,
                Url = movie.CoverUrl,
                Year = movie.Year,
                Rating = movie.Rating,
                Genre = genre,
                Plot = movie.Plot
            });
        }
    }
}