using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WcfServiceLab3
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "MovieService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select MovieService.svc or MovieService.svc.cs at the Solution Explorer and start debugging.
    public class MovieService : IMovieService
    {
        /* Instruktioner från Backe på handledningen
        1. En metod som skapar rotnoden/omslutande noden<Movies>.Itererar över varje film i _movies.
        2. För varje film i _movies -> Anropa en annan metod som skapar en<Movie>
        3. Metoden som skapar en <Movie> behöver även hämta information från _actors och 
        _genres så att vi byter ut ID mot det faktiska värdet*/

        private string movies = "aHR0cDovL3ByaXZhdC5iYWhuaG9mLnNlL3diNzE0ODI5L2pzb24vbW92aWVzLmpzb24=";
        private string actors = "aHR0cDovL3ByaXZhdC5iYWhuaG9mLnNlL3diNzE0ODI5L2pzb24vYWN0b3JzLmpzb24=";
        private string genres = "aHR0cDovL3ByaXZhdC5iYWhuaG9mLnNlL3diNzE0ODI5L2pzb24vZ2VucmUuanNvbg==";

        private List<JObject> _movies;
        private List<JObject> _actors;
        private List<JObject> _genres;

        private XElement xMovies;

        /// <summary>
        /// Gets all movies
        /// </summary>
        /// <returns>XElement</returns>
        public XElement GetAllMovies()
        {
            UpdateAll();

            xMovies = new XElement("Movies");

            foreach (JObject movie in _movies)
            {
                MakeMovieXML(movie, xMovies);
            }

            return xMovies;
            
        }

        /// <summary>
        /// Gets the top ranked 10 movies 
        /// </summary>
        /// <returns>XElement</returns>
        public XElement GetTopTenMovies()
        {
            UpdateAll();

            xMovies = new XElement("Movies");

            var topTenJObject = (from m in _movies
                                 orderby m["Rating"] descending
                                 select m).Take(10);

            foreach (JObject movie in topTenJObject)
            {
                MakeMovieXML(movie, xMovies);
            }

            return xMovies;
           
        }
           
        private void MakeMovieXML(JObject movie, XElement xmlMovies)
        {
            XElement xmlMovie = new XElement("Movie");
            foreach (var element in movie)
            {
                if (element.Value.Count() == 0)
                {
                    XElement node = new XElement(element.Key, element.Value.ToString());
                    xmlMovie.Add(node);
                }
                else if (element.Key == "Genre")
                {
                    XElement genres = new XElement("Genres");
                    foreach (var genreID in element.Value)
                    {
                        XElement node = new XElement("Genre", GetGenre(genreID.ToString()));
                        genres.Add(node);
                    }
                    xmlMovie.Add(genres);
                }
                else if (element.Key == "Actors")
                {
                    XElement actors = new XElement("Actors");
                    foreach (var actorID in element.Value)
                    {
                        XElement node = new XElement("Actor", GetActor(actorID.ToString()));
                        actors.Add(node);
                    }
                    xmlMovie.Add(actors);
                }
            }
            xmlMovies.Add(xmlMovie);
        }

        private string GetActor(string val)
        {
            return (from a in _actors
                    where a["ID"].ToString() == val
                    select a["Name"]).First().ToString();
        }

        private string GetGenre(string val)
        {
            return (from g in _genres
                    where g["ID"].ToString() == val
                    select g["Name"]).First().ToString();
        }

        private void UpdateAll()
        {
            if(_movies == null)
            {
                using (WebClient webClient = new WebClient())
                {
                    _movies = JsonConvert.DeserializeObject<List<JObject>>(webClient.DownloadString(Encoding.UTF8.GetString(Convert.FromBase64String(movies))));

                    _actors = JsonConvert.DeserializeObject<List<JObject>>(webClient.DownloadString(Encoding.UTF8.GetString(Convert.FromBase64String(actors))));

                    _genres = JsonConvert.DeserializeObject<List<JObject>>(webClient.DownloadString(Encoding.UTF8.GetString(Convert.FromBase64String(genres))));
                }
            }
               
        }
    }
}
