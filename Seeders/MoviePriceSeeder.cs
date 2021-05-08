using System;
using System.Linq;
using lib.Library.Services;



namespace Seeders
{
    class MoviePriceSeeder
    {
        MovieService movieService = new MovieService();
        ShopMoviePriceService shopMoviePriceService = new ShopMoviePriceService();

        public void Run()
        {
            var rand = new Random();
            int count = movieService.All().Count();

            shopMoviePriceService.DeleteAll();

            for (int i = 1; i <= count; i++)
            {
                shopMoviePriceService.Add(i, (decimal) rand.NextDouble() * 35);
            }
        }
    }
}