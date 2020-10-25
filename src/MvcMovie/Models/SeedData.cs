using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MvcMovie.Controllers;
using MvcMovie.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using MvcMovie.Utils;
using Newtonsoft.Json.Linq;

namespace MvcMovie.Models
{
    public static class SeedData
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Initialize(IServiceProvider serviceProvider)
        {
            log4net.Config.XmlConfigurator.Configure();
            var context = new MvcMovieContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<MvcMovieContext>>());

            {   
                // don't seed the original movie models if exist
                if (!context.Movie.Any())
                {
                    context.Movie.AddRange(
                        new Movie
                        {
                            Title = "When Harry Met Sally",
                            ReleaseDate = DateTime.Parse("1989-2-12"),
                            Genre = "Romantic Comedy",
                            Price = 7.99M
                        },

                        new Movie
                        {
                            Title = "Ghostbusters ",
                            ReleaseDate = DateTime.Parse("1984-3-13"),
                            Genre = "Comedy",
                            Price = 8.99M
                        },
                        new Movie
                        {
                            Title = "Rio Bravo",
                            ReleaseDate = DateTime.Parse("1959-4-15"),
                            Genre = "Western",
                            Price = 3.99M
                        });

                }


                // don't seed the Json movie models if exist
                if (!context.Movies.Any())
                {
                    var controller = new MvcMoviesController(context);
                    var JsonMovieList = controller.GetMvcMoviesFromJson();
                    var seededMoviesList = new List<Movies>();

                    // use automapper to map Json response to Movie objects
                    foreach (dynamic m in JsonMovieList)
                    {
                        var mapper = new AutoMapperUtils();

                        var _movie = mapper.MapJsonMovieToEntityMoviesModel(JObject.FromObject(m));
                        seededMoviesList.Add(_movie);
                    }    
                    context.Movies.AddRange(seededMoviesList);
                    context.SaveChanges();
                    // create a new async thread to fill the Foreign Key Ids to run in the background
                    // this will take some time to complete and not needed for the movie search
                    Task.Factory.StartNew(() => SetContextIdsAsync(context));
                }
            }
        }

        public static Task SetContextIdsAsync(MvcMovieContext context)
        {
            var timer = new Stopwatch();
            timer.Start();
            foreach (Movies m in context.Movies)
            {
                var movieId = (from mov in context.Movies where mov.Title.Equals(m.Title) select mov.Id).FirstOrDefault();
                var info = m.Info;
                info.MovieId = m.Id;
            }
            context.SaveChanges();
            foreach (Movies m2 in context.Movies)
            { 
                var info2 = m2.Info;
                var infoId = (from mov in context.Movies where mov.Id.Equals(info2.MovieId) select mov.Info.Id).FirstOrDefault();
                var actors = m2.Info.Actors;
                var directors = m2.Info.Directors;
                var genres = m2.Info.Genres;
                foreach (Actors a in actors) { a.InfoId = infoId; }
                foreach (Directors d in directors) { d.InfoId = infoId; }
                foreach (Genres g in genres) { g.InfoId = infoId; }
            }
            context.SaveChanges();
            timer.Stop();
            log.Info(string.Format("SetContextIdsAsync - completed adding Ids to Movies in separate thread and saving to DB:  {0} ms have elapsed.", timer.ElapsedMilliseconds.ToString()));
            // Dispose this context, we don't need it anymore it was only for seeding data
            context.Dispose();
            return null;
        }
    }
}