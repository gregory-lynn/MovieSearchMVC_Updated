using System.Linq;
using Xunit;
using System;
using MvcMovie.Models.Entities;

namespace MvcMovieUnitTests
{
    public class MovieUnitTests
    {
        private Helpers _helper = new Helpers();
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [Fact]
        public void DeleteAllMoviesTest()
        {
            try
            {
                _helper.DeleteAllMovies();
                _helper.GetAllMovies();

                Assert.Empty(_helper.AllMovies);
            }
            catch (Exception e)
            {
                var test = e.Message;
                // add logging code here
            }

        }

        [Fact]
        public void CheckAllMovieInfoObjectsForIdsTest()
        {
            try
            {
                _helper.GetAllMovies();
                var lstMovieInfosWithoutIds = (from m in _helper.AllMovies where (m.Info.MovieId != m.Id) select m).ToList();
                var movieCountWithoutIds = lstMovieInfosWithoutIds.Count();
                Assert.Equal(0, movieCountWithoutIds);
            }
            catch (Exception e)
            {
                var test = e.Message;
                // add logging code here
            }

        }

        [Fact]
        public void GetMoviesTest()
        {
            try
            {
                Assert.NotEmpty(_helper.AllMovies);
                //_helper.AddMovies(_helper.TestMovies);
                Movies JsonMovie = (from m in _helper.TestMovies select m).FirstOrDefault();
                Movies TestMovie = (from m in _helper.AllMovies where m.Title.Equals(JsonMovie.Title) select m).FirstOrDefault();
                Assert.NotNull(TestMovie);
            }
            catch (Exception e)
            {
                var test = e.Message;
                // add logging code here
            }
        }
    }
}
