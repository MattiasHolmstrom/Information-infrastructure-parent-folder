using MVC_Lab3.Models;
using MVC_Lab3.MovieServiceReference;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Xml.Linq;

namespace MVC_Lab3.Context
{
    public class MovieContext : DbContext
    {
        public MovieContext()
            : base("MovieContext")
        {

        }

        public DbSet<Movie> Movies { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
    public class ServiceCommunication
    {

        MovieServiceClient api;

        public ServiceCommunication()
        {
            api = new MovieServiceClient();
        }

        public XElement GetAllMovies()
        {
            return api.GetAllMovies();
        }
    }
}