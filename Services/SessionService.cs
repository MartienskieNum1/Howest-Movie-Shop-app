using System.Collections.Generic;
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
    }
}