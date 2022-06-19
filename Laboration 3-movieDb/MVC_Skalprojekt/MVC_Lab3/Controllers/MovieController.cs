using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using MVC_Lab3.Context;
using MVC_Lab3.Models;
using MVC_Lab3.MovieServiceReference;
using PagedList;

namespace MVC_Lab3.Controllers
{
    public class MovieController : Controller
    {
        private MovieContext db = new MovieContext();
        // GET: Movie
        public ActionResult Index(string searchtitle, string orgtitle, string releaseyr, string genre, int? page, string currentFilterTitle, string currentFilterOrgTitle, string currentFilterRelease, string currentFilterGenre)
        {  
            if (searchtitle != null)
            {
                page = 1;
            }
            else
            {
                searchtitle = currentFilterTitle;
            }
           
            ViewBag.CurrentFilterTitle = searchtitle;

            if (orgtitle != null)
            {
                page = 1;
            }
            else
            {
                orgtitle = currentFilterOrgTitle;
            }
            ViewBag.CurrentFilterOrgTitle = orgtitle;

            if (releaseyr != null)
            {
                page = 1;
            }
            else
            {
                releaseyr = currentFilterRelease;
            }
            ViewBag.CurrentFilterRelease = releaseyr;

            if (genre != null)
            {
                page = 1;
            }
            else
            {
                genre = currentFilterGenre;
            }
            ViewBag.CurrentFilterGenre = genre;

            var movies = from m in db.Movies
                         select m;

            
            if (!String.IsNullOrEmpty(searchtitle))
            {
                searchtitle = searchtitle.Trim();
                movies = movies.Where(s => s.Title.Contains(searchtitle));
            }
            if(!String.IsNullOrEmpty(orgtitle))
            {
                orgtitle = orgtitle.Trim();
                movies = movies.Where(s => s.OriginalTitle.Contains(orgtitle));
            }
            if (!String.IsNullOrEmpty(releaseyr))
            {
                releaseyr = releaseyr.Trim();
                int ryear;
                if (Int32.TryParse(releaseyr, out ryear))
                {
                    movies = movies.Where(s => s.ReleaseYear.Equals(ryear));
                }               
            }
            if (!String.IsNullOrEmpty(genre))
            {
                genre = genre.Trim();
                movies = movies.Where(s => s.Genre.Contains(genre));
            }

            movies = movies.OrderBy(x => x.Title);


            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(movies.ToPagedList(pageNumber, pageSize));
  
        }

        // GET: Movie/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // GET: Movie/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Movie/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MovieID,Title,OriginalTitle,ReleaseYear,Rating,Synopsis,Genre,Actors")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                db.Movies.Add(movie);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(movie);
        }

        // GET: Movie/Edit/5
        public ActionResult Edit(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: Movie/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MovieID,Title,OriginalTitle,ReleaseYear,Rating,Synopsis,Genre,Actors")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                db.Entry(movie).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(movie);
        }

        // GET: Movie/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: Movie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Movie movie = db.Movies.Find(id);
            db.Movies.Remove(movie);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
    //public class ServiceCommunication
    //{
    //    private XElement _result;
    //    public XElement Result;

    //    MovieServiceClient api;

    //    public ServiceCommunication()
    //    {
    //        api = new MovieServiceClient();

    //    }

    //    public void GetAllMovies()
    //    {

    //      api.UpdateMovies();
    //    }
    //}
    
}
