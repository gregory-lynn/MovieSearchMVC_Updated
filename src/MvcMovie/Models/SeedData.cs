using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using MvcMovie.Models.Entities;
using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace MvcMovie.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MvcMovieContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<MvcMovieContext>>()))
            {   // don't seed the original movie models if exist
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

                // don't seed the new movie models if exist
                if (!context.Movies.Any())
                {
                    context.Movies.AddRange(
                    new Entities.Movies
                    {
                        Title = "Rush",
                        Year = "2013",
                        //Info = GetInfo1()
                    },
                    new Entities.Movies
                    {
                        Title = "Prisoners",
                        Year = "2013",
                        //Info = GetInfo2()
                    },
                    new Entities.Movies
                    {
                        Title = "The Hunger Games: Catching Fire",
                        Year = "2013",
                        //Info = GetInfo3()
                    },
                    new Entities.Movies
                    {
                        Title = "Thor: The Dark World",
                        Year = "2013",
                        //Info = GetInfo4()
                    });
                    
                    // save movie entities
                    context.SaveChanges();

                    var movieEntity1 = (from m in context.Movies where m.Title.Equals("Rush") select m).FirstOrDefault();
                    var movieEntity2 = (from m in context.Movies where m.Title.Equals("Prisoners") select m).FirstOrDefault();
                    var movieEntity3 = (from m in context.Movies where m.Title.Equals("The Hunger Games: Catching Fire") select m).FirstOrDefault();
                    var movieEntity4 = (from m in context.Movies where m.Title.Equals("Thor: The Dark World") select m).FirstOrDefault();
                   
                    var info1 = GetInfo1(movieEntity1);
                    var info2 = GetInfo2(movieEntity2);
                    var info3 = GetInfo3(movieEntity3);
                    var info4 = GetInfo4(movieEntity4);

                    movieEntity1.Info = info1;
                    movieEntity2.Info = info2;
                    movieEntity3.Info = info3;
                    movieEntity4.Info = info4;

                    // save movie entity info details
                    context.SaveChanges();

                    var directors1 = GetDirectors1(movieEntity1.Info);
                    var directors2 = GetDirectors2(movieEntity2.Info);
                    var directors3 = GetDirectors3(movieEntity3.Info);
                    var directors4 = GetDirectors4(movieEntity4.Info);
                    var genres1 = GetGenres1(movieEntity1.Info);
                    var genres2 = GetGenres2(movieEntity2.Info);
                    var genres3 = GetGenres3(movieEntity3.Info);
                    var genres4 = GetGenres4(movieEntity4.Info);
                    var actors1 = GetActors1(movieEntity1.Info);
                    var actors2 = GetActors2(movieEntity2.Info);
                    var actors3 = GetActors3(movieEntity3.Info);
                    var actors4 = GetActors4(movieEntity4.Info);

                    movieEntity1.Info.Directors = directors1;
                    movieEntity1.Info.Genres = genres1;
                    movieEntity1.Info.Actors = actors1;
                    movieEntity2.Info.Directors = directors2;
                    movieEntity2.Info.Genres = genres2;
                    movieEntity2.Info.Actors = actors2;
                    movieEntity3.Info.Directors = directors3;
                    movieEntity3.Info.Genres = genres3;
                    movieEntity3.Info.Actors = actors3;
                    movieEntity4.Info.Directors = directors4;
                    movieEntity4.Info.Genres = genres4;
                    movieEntity4.Info.Actors = actors4;

                    // save movie entity info directors, genres, actors details
                    context.SaveChanges();
                }
            }
        }
        #region Movie1
        private static Info GetInfo1(Entities.Movies movie)
        {
            var info = new Info
            {
                MovieId = movie.Id,
                //Directors = GetDirectors1(),
                ReleaseDate = DateTime.Parse("2013-09-02T00:00:00Z"),
                Rating = 7.0m,
                //Genres = GetGenres1(),
                ImageUrl = "https://ia.media-imdb.com/images/M/MV5BMTQyMDE0MTY0OV5BMl5BanBnXkFtZTcwMjI2OTI0OQ@@._V1_SX400_.jpg",
                Plot = "A re-creation of the merciless 1970s rivalry between Formula One rivals James Hunt and Niki Lauda.",
                Rank = "2",
                RunningTime = "9009",
                //Actors = GetActors1()
            };
            return info;
        }

        private static List<Directors> GetDirectors1(Entities.Info info)
        {
            var directors = new List<Directors> {
                new Directors
                {
                    InfoId = info.Id,
                    Director = "Ron Howard"
                }
                // The rest of the categories here... add comma before new brackets
            };
            return directors;
        }
        private static List<Genres> GetGenres1(Entities.Info info)
        {
            var genres = new List<Genres> {
                new Genres
                {
                    InfoId = info.Id,
                    Genre = "Action"
                },
                new Genres
                {
                    InfoId = info.Id,
                    Genre = "Biography"
                },
                new Genres
                {
                    InfoId = info.Id,
                    Genre = "Drama"
                },
                new Genres
                {
                    InfoId = info.Id,
                    Genre = "Sport"
                },
            };
            return genres;
        }
        private static List<Actors> GetActors1(Entities.Info info)
        {
            var actors = new List<Actors> {
                new Actors
                {
                    InfoId = info.Id,
                    Actor = "Daniel Bruhl"
                },
                new Actors
                {
                    InfoId = info.Id,
                    Actor = "Chris Hemsworth"
                },
                new Actors
                {
                    InfoId = info.Id,
                    Actor = "Olivia Wilde"
                }
            };
            return actors;
        }
        #endregion
        #region Movie2
        private static Info GetInfo2(Entities.Movies movie)
        {
            var info = new Info
            {
                MovieId = movie.Id,
                //Directors = GetDirectors2(),
                ReleaseDate = DateTime.Parse("2013-08-30T00:00:00Z"),
                Rating = 8.2m,
                //Genres = GetGenres2(),
                ImageUrl = "https://ia.media-imdb.com/images/M/MV5BMTg0NTIzMjQ1NV5BMl5BanBnXkFtZTcwNDc3MzM5OQ@@._V1_SX400_.jpg",
                Plot = "When Keller Dover's daughter and her friend go missing, he takes matters into his own hands as the police pursue multiple leads and the pressure mounts. But just how far will this desperate father go to protect his family?",
                Rank = "3",
                RunningTime = "9180",
                //Actors = GetActors2()
            };
            return info;
        }

        private static List<Directors> GetDirectors2(Entities.Info info)
        {
            var directors = new List<Directors> {
                new Directors
                {
                    InfoId = info.Id,
                    Director = "Denis Villeneuve"
                }
                // The rest of the categories here... add comma before new brackets
            };
            return directors;
        }
        private static List<Genres> GetGenres2(Entities.Info info)
        {
            var genres = new List<Genres> {
                new Genres
                {
                    InfoId = info.Id,
                    Genre = "Crime"
                },
                new Genres
                {
                    InfoId = info.Id,
                    Genre = "Drama"
                },
                new Genres
                {
                    InfoId = info.Id,
                    Genre = "Thriller"
                },
            };
            return genres;
        }
        private static List<Actors> GetActors2(Entities.Info info)
        {
            var actors = new List<Actors> {
                new Actors
                {
                    InfoId = info.Id,
                    Actor = "Hugh Jackman"
                },
                new Actors
                {
                    InfoId = info.Id,
                    Actor = "Jake Gyllenhaal"
                },
                new Actors
                {
                    InfoId = info.Id,
                    Actor = "Viola Davis"
                }
            };
            return actors;
        }
        #endregion
        #region Movie3
        private static Info GetInfo3(Entities.Movies movie)
        {
            var info = new Info
            {
                MovieId = movie.Id,
                //Directors = GetDirectors3(),
                ReleaseDate = DateTime.Parse("2013-11-11T00:00:00Z"),
                Rating = 4.0m,
                //Genres = GetGenres3(),
                ImageUrl = "https://ia.media-imdb.com/images/M/MV5BMTAyMjQ3OTAxMzNeQTJeQWpwZ15BbWU4MDU0NzA1MzAx._V1_SX400_.jpg",
                Plot = "Katniss Everdeen and Peeta Mellark become targets of the Capitol after their victory in the 74th Hunger Games sparks a rebellion in the Districts of Panem.",
                Rank = "4",
                RunningTime = "8760",
                //Actors = GetActors3()
            };
            return info;
        }

        private static List<Directors> GetDirectors3(Entities.Info info)
        {
            var directors = new List<Directors> {
                new Directors
                {
                    InfoId = info.Id,
                    Director = "Francis Lawrence"
                }
                // The rest of the categories here... add comma before new brackets
            };
            return directors;
        }
        private static List<Genres> GetGenres3(Entities.Info info)
        {
            var genres = new List<Genres> {
                new Genres
                {
                    InfoId = info.Id,
                    Genre = "Action"
                },
                new Genres
                {
                    InfoId = info.Id,
                    Genre = "Adventure"
                },
                new Genres
                {
                    InfoId = info.Id,
                    Genre = "Sci-Fi"
                },
                new Genres
                {
                    InfoId = info.Id,
                    Genre = "Thriller"
                }
            };
            return genres;
        }
        private static List<Actors> GetActors3(Entities.Info info)
        {
            var actors = new List<Actors> {
                new Actors
                {
                    InfoId = info.Id,
                    Actor = "Jennifer Lawrence"
                },
                new Actors
                {
                    InfoId = info.Id,
                    Actor = "Josh Hutcherson"
                },
                new Actors
                {
                    InfoId = info.Id,
                    Actor = "Liam Hemsworth"
                }
            };
            return actors;
        }
        #endregion
        #region Movie4
        private static Info GetInfo4(Entities.Movies movie)
        {
            var info = new Info
            {
                MovieId = movie.Id,
                //Directors = GetDirectors4(),
                ReleaseDate = DateTime.Parse("2013-10-30T00:00:00Z"),
                Rating = 0.0m,
                //Genres = GetGenres4(),
                ImageUrl = "https://ia.media-imdb.com/images/M/MV5BMTQyNzAwOTUxOF5BMl5BanBnXkFtZTcwMTE0OTc5OQ@@._V1_SX400_.jpg",
                Plot = "Faced with an enemy that even Odin and Asgard cannot withstand, Thor must embark on his most perilous and personal journey yet, one that will reunite him with Jane Foster and force him to sacrifice everything to save us all.",
                Rank = "5",
                RunningTime = null,
                //Actors = GetActors4()
            };
            return info;
        }

        private static List<Directors> GetDirectors4(Entities.Info info)
        {
            var directors = new List<Directors> {
                new Directors
                {
                    InfoId = info.Id,
                    Director = "Alan Taylor"
                }
                // The rest of the categories here... add comma before new brackets
            };
            return directors;
        }
        private static List<Genres> GetGenres4(Entities.Info info)
        {
            var genres = new List<Genres> {
                new Genres
                {
                    InfoId = info.Id,
                    Genre = "Action"
                },
                new Genres
                {
                    InfoId = info.Id,
                    Genre = "Adventure"
                },
                new Genres
                {
                    InfoId = info.Id,
                    Genre = "Fantasy"
                }
            };
            return genres;
        }
        private static List<Actors> GetActors4(Entities.Info info)
        {
            var actors = new List<Actors> {
                new Actors
                {
                    InfoId = info.Id,
                    Actor = "Natalie Portman"
                },
                new Actors
                {
                    InfoId = info.Id,
                    Actor = "Chris Hemsworth"
                },
                new Actors
                {
                    InfoId = info.Id,
                    Actor = "Tom Hiddleston"
                }
            };
            return actors;
        }
        #endregion
    }
}