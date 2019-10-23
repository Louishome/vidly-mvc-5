using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    public class MoviesController : Controller
    {
        private ApplicationDbContext m_dbContext;

        public MoviesController()
        {
            m_dbContext = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            m_dbContext.Dispose();
        }

        public ViewResult Index()
        {
            var movies = GetMovies();

            return View(movies);    
        }

        private IEnumerable<Movie> GetMovies() => m_dbContext.Movies.Include(m => m.GenreType).ToList();

        public ActionResult Details(int id)
        {
            var movie = GetMovies().SingleOrDefault(m => m.Id == id);
            if (movie == null)
            {
                return HttpNotFound();
            }

            return View(movie);
        }

        public ActionResult New()
        {
            var modelView = new MovieFormViewModel
            {
                GenreTypes = m_dbContext.GenreTypes.ToList()
            };
            return View("MovieForm", modelView);
        }

        public ActionResult Save(MovieFormViewModel viewModel)
        {
            if (viewModel.Movie.Id == 0)
            {
                m_dbContext.Movies.Add(viewModel.Movie);
            }

            m_dbContext.SaveChanges();
            return RedirectToAction("Index", "Movies");
        }
    }
}