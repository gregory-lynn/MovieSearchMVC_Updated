using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MvcMovie;
using MvcMovie.Controllers;
using MvcMovie.Models;
using Newtonsoft.Json;
using Xunit;

namespace MvcMovieUnitTests
{
    class Helpers
    {
        #region Private Properties
        private List<MvcMovie.Models.Entities.Movies> _AllMovies;
        private List<MvcMovie.Models.Entities.Movies> _TestMovies;
        private List<MvcMovie.Models.Entities.Info> _infos;
        private List<MvcMovie.Models.Entities.Actors> _actors;
        private List<MvcMovie.Models.Entities.Directors> _directors;
        private List<MvcMovie.Models.Entities.Genres> _genres;
        private MvcMovieContext _context;
        #endregion
        #region Public Properties
        public List<MvcMovie.Models.Entities.Movies> AllMovies
        {
            get { return _AllMovies; }
            set { _AllMovies = value; }
        }
        public List<MvcMovie.Models.Entities.Movies> TestMovies
        {
            get { return _TestMovies; }
            set { _TestMovies = value; }
        }
        #endregion
        #region Constructor
        public Helpers()
        {
            Startup();
            GetAllMovies();
            GetTestMoviesFromJson();
            //DeleteAllTestMovies();
        }
        #endregion

        // update properties with entity model
        public void AddMovie(MvcMovie.Models.Entities.Movies movie)
        {
            //List<MvcMovie.Models.Entities.Movies> tmpList = new List<MvcMovie.Models.Entities.Movies>();
            //tmpList.Add(movie);
            // AddMovies(tmpList);
            _context.Add(
                new MvcMovie.Models.Entities.Movies
                {
                    // update properties with entity model

                    //Title = movie.Title,
                    //ReleaseDate = movie.ReleaseDate,
                    //Genre = movie.Genre,
                    //Price = movie.Price
                });
            _context.SaveChanges();
        }
        public void AddMovies(List<MvcMovie.Models.Entities.Movies> movies)
        {
            //_context.Movie.AddRange(movies);
            //_context.SaveChanges();
            foreach (MvcMovie.Models.Entities.Movies m in movies)
            {
                // update properties with entity model
                AddMovie(m);
            }
        }
        public void DeleteAllMovies()
        {
            foreach (MvcMovie.Models.Entities.Movies m in AllMovies)
            {
                _context.Movies.Remove(m);
                _context.SaveChanges();
            }
        }
        //public void DeleteAllTestMovies()
        //{
        //    foreach (MvcMovie.Models.Entities.Movies m in TestMovies)
        //    {
        //        var tmpMovie = (from tmp in AllMovies where tmp.Title.Equals(m.Title) select tmp).FirstOrDefault();
        //        if (tmpMovie != null)
        //        {
        //            _context.Movies.Remove(tmpMovie);
        //            _context.SaveChanges();
        //        }
        //    }
        //}
        public void GetAllMovies()
        {
            //AllMovies = (from m in _context.Movies orderby m.Info.ReleaseDate select m).ToList();
            AllMovies = _context.Movies.Include(x => x.Info)
                .Include(x => x.Info.Directors)
                .Include(x => x.Info.Genres).Include(x => x.Info.Actors).ToList();
        }

        public void GetTestMoviesFromJson()
        {
            try
            {
                using StreamReader file = (StreamReader)GetInputFile("TestMovies.json");
                JsonSerializer serializer = new JsonSerializer();
                TestMovies = (List<MvcMovie.Models.Entities.Movies>)serializer.Deserialize(file, typeof(List<MvcMovie.Models.Entities.Movies>));
            }
            catch (Exception e)
            {
                var test = e.Message;
            }
        }

        public static TextReader GetInputFile(string filename)
        {
            Assembly thisAssembly = Assembly.GetExecutingAssembly();
            string path = "MvcMovieUnitTests";
            return new StreamReader(thisAssembly.GetManifestResourceStream(path + "." + filename));
        }
        private void Startup()
        {
            var args = new string[0];
            var webHost = WebHost.CreateDefaultBuilder().UseStartup<Startup>().Build();
            Assert.NotNull(webHost);
            Assert.NotNull(webHost.Services.GetRequiredService<MvcMovieContext>());
            _context = webHost.Services.GetService<MvcMovieContext>();
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<MvcMovieContext>();
                context.Database.Migrate();
                SeedData.Initialize(services);
            }
            try
            {
                webHost.Start();
                _context.SaveChanges();
            }
            catch
            {
                // do nothing web host already started
            }
        }
    }
    public class Startup : MvcMovie.Startup
    {
        public Startup(IConfiguration config) : base(config) { }
    }
}
