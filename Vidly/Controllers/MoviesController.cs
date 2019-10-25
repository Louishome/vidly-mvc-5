using System;
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
                Movie = new Movie(),
                GenreTypes = m_dbContext.GenreTypes.ToList()
            };
            return View("MovieForm", modelView);
        }

        public ActionResult Edit(int id)
        {
            var movie = m_dbContext.Movies.SingleOrDefault(m => m.Id == id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            
            var modelView = new MovieFormViewModel
            {
                Movie = movie,
                GenreTypes = m_dbContext.GenreTypes.ToList()
            };
            return View("MovieForm", modelView);
        }

        public ActionResult Save(MovieFormViewModel viewModel)
        {
            if (viewModel.Movie.Id == 0)
            {
                viewModel.Movie.DateAdded = DateTime.Now;
                m_dbContext.Movies.Add(viewModel.Movie);
            }
            else
            {
                var existingMovie = m_dbContext.Movies.Single(m => m.Id == viewModel.Movie.Id);
                existingMovie.DateAdded = viewModel.Movie.DateAdded;
                existingMovie.GenreId = viewModel.Movie.GenreId;
                existingMovie.Name = viewModel.Movie.Name;
                existingMovie.NumberInStock = viewModel.Movie.NumberInStock;
                existingMovie.ReleaseDate = viewModel.Movie.ReleaseDate;
            }

            m_dbContext.SaveChanges();
            return RedirectToAction("Index", "Movies");
        }
    }
}