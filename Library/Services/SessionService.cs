using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using app.Models;
using lib.Library.Models;

namespace app.Services
{
    public class SessionService
    {
        public void StoreMovies(ISession session, IEnumerable<Movie> movies)
        {
            session.SetString("movies", JsonSerializer.Serialize(movies));
        }

        public IEnumerable<Movie> GetMovies(ISession session)
        {
            string moviesJson = session.GetString("movies");
            if (!string.IsNullOrEmpty(moviesJson))
            {
                return JsonSerializer.Deserialize<IEnumerable<Movie>>(moviesJson);
            } else
            {
                return null;
            }
        }

        public void StorePrices(ISession session, IEnumerable<ShopMoviePrice> moviePrices)
        {
            session.SetString("moviePrices", JsonSerializer.Serialize(moviePrices));
        }

        public IEnumerable<ShopMoviePrice> GetPrices(ISession session)
        {
            string pricesJson = session.GetString("moviePrices");
            if (!string.IsNullOrEmpty(pricesJson))
            {
                return JsonSerializer.Deserialize<IEnumerable<ShopMoviePrice>>(pricesJson);
            } else
            {
                return null;
            }
        }

        public decimal GetPriceForMovieId(ISession session, int movieId)
        {
            return GetPrices(session)
                    .Where(p => p.MovieId == movieId)
                    .Select(p=> p.UnitPrice)
                    .First();
        }

        public Movie GetMovieForMovieId(ISession session, int movieId)
        {
            return GetMovies(session)
                .Where(m => m.Id == movieId)
                .First();
        }

        public IEnumerable<Movie> GetMoviesForMovieIds(ISession session, List<int> ids)
        {
            return GetMovies(session)
                .Where(m => ids.Contains(Convert.ToInt32(m.Id)))
                .ToList();
        }

        public void SetInOrderProcess(ISession session, bool inProcess)
        {
            session.SetString("inOrderProcess", inProcess.ToString());
        }

        public bool GetInOrderProcess(ISession session)
        {
            string inProcess = session.GetString("inOrderProcess");
            if (!string.IsNullOrEmpty(inProcess))
            {
                return Convert.ToBoolean(inProcess);
            } else
            {
                return false;
            }
        }

        public void SetFilter(ISession session, MoviesViewModel model)
        {
            session.SetString("filter", JsonSerializer.Serialize(model));
        }

        public MoviesViewModel GetFilter(ISession session)
        {
            string filter = session.GetString("filter");
            if (!string.IsNullOrEmpty(filter))
            {
                return JsonSerializer.Deserialize<MoviesViewModel>(filter);
            } else
            {
                return null;
            }
        }

        public void AddToCart(ISession session, int id)
        {
            List<int> previousCart = GetCart(session);
            List<int> newCart = new List<int>();
            
            if (previousCart == null)
            {
                newCart.Add(id);
                session.SetString("cart", JsonSerializer.Serialize(newCart));
            } else
            {
                if (!previousCart.Contains(id))
                {
                    previousCart.Add(id);
                    session.SetString("cart", JsonSerializer.Serialize(previousCart));
                }
            }
        }

        public List<int> GetCart(ISession session)
        {
            string cartJson = session.GetString("cart");
            if (!string.IsNullOrEmpty(cartJson))
            {
                return JsonSerializer.Deserialize<List<int>>(cartJson);   
            } else
            {
                return null;
            }
        }

        public void ClearCart(ISession session)
        {
            session.Remove("cart");
        }

        public int GetCartAmount(ISession session)
        {
            if (GetCart(session) == null)
            {
                return 0;
            } else
            {
                return GetCart(session).Count;
            }
        }

        public void ClearSession(ISession session)
        {
            session.Clear();
        }
    }
}