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
    }
}