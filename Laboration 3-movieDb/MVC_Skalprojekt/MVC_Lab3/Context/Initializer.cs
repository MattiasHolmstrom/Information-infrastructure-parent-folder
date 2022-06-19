using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Xml.Linq;
using MVC_Lab3.Models;

namespace MVC_Lab3.Context
{
    public class Initializer : DropCreateDatabaseAlways<MovieContext>
    {
        protected override void Seed(MovieContext context)
        {
            ServiceCommunication sc = new ServiceCommunication();
            var movies = sc.GetAllMovies();

            foreach (XElement x in movies.Elements("Movie"))
            {
                Movie movie = new Movie();
                movie.SetMovieFromXML(x);
                context.Movies.Add(movie);
            }

            context.SaveChanges();
        }
    }
}