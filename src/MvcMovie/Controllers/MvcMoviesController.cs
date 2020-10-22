using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Migrations;
using MvcMovie.Models;
using Newtonsoft.Json;

namespace MvcMovie.Controllers
{
    public class MvcMoviesController : Controller
    {
        private readonly MvcMovieContext _context;

        public MvcMoviesController(MvcMovieContext context)
        {
            _context = context;
        }

        private List<MvcMovie.Models.Entities.Movies> _AllMovies;
        public List<MvcMovie.Models.Entities.Movies> AllMovies
        {
            get { return _AllMovies; }
            set { _AllMovies = value; }
        }

        // GET: Movies
        public async Task<IActionResult> Index(string searchString)
        {
            // Use LINQ to get list of titles.
            IQueryable<string> genreQuery = from m in _context.Movies
                                            orderby m.Info.ReleaseDate
                                            select m.Title;


            _AllMovies = _context.Movies.Include(x => x.Info)
                .Include(x => x.Info.Directors)
                .Include(x => x.Info.Genres).Include(x => x.Info.Actors).ToList();

            var movies = from m in _AllMovies
                         select m;

            if (!string.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.Title.Contains(searchString));
            }

            // change default sort order based on requirements
            movies = movies.OrderByDescending(m => m.Info.ReleaseDate);

            var movieEntityVM = new MoviesEntityViewModel
            {
               //Movies = await movies.AsQueryable().ToListAsync()
               Movies = await Task.FromResult(movies.ToList())
            };
            
            return View(movieEntityVM);
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            _AllMovies = _context.Movies.Include(x => x.Info)
                .Include(x => x.Info.Directors)
                .Include(x => x.Info.Genres).Include(x => x.Info.Actors).ToList();

            //var movie = from m in AllMovies where m.Id.Equals(id)
            //             select m;
            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReleaseDate")] Models.Entities.Movies movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Edit));
            }
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }

        public List<Models.Entities.Movies> GetMvcMoviesFromJson()
        {
            try
            {
                using StreamReader file = (StreamReader)GetInputFile("moviedata.json");
                JsonSerializer serializer = new JsonSerializer();
                return (List<MvcMovie.Models.Entities.Movies>)serializer
                    .Deserialize(file, typeof(List<MvcMovie.Models.Entities.Movies>));
            }
            catch (Exception e)
            {
                var test = e.Message;
                return null;
            }
        }
        public static TextReader GetInputFile(string filename)
        {
            Assembly thisAssembly = Assembly.GetExecutingAssembly();
            string path = "MvcMovieUnitTests";
            return new StreamReader(thisAssembly.GetManifestResourceStream(path + "." + filename));
        }
    }
}
