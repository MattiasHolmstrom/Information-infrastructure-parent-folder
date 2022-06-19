using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace MVC_Lab3.Models
{
    public class Movie
    {

        //Add your properties here.
        //Add your properties here.
        public int MovieID { get; set; }
        public string Title { get; set; }
        public string OriginalTitle { get; set; }
        public int ReleaseYear { get; set; }
        public double Rating { get; set; }
        public string Synopsis { get; set; }
        public string Genre { get; set; }
        public string Actors { get; set; }
        
        public void SetMovieFromXML(XElement xml)
        {
            Title = xml.Element("Title").Value;
            OriginalTitle = xml.Element("OriginalTitle").Value;
            ReleaseYear = (int)xml.Element("ReleaseYear");
            Rating = double.Parse(xml.Element("Rating").Value.Replace(",", "."));
            Synopsis = xml.Element("Synopsis").Value;
            Genre = string.Join(", ", xml.Element("Genres").Elements("Genre").Select(x => x.Value).ToArray());
            Actors = string.Join(", ", xml.Element("Actors").Elements("Actor").Select(x => x.Value).ToArray());
        }
    }
}