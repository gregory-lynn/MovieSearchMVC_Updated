using System;
using System.Collections.Generic;
using AutoMapper;
using MvcMovie.Models.Entities;
using Newtonsoft.Json.Linq;

namespace MvcMovie.Utils
{
    public class AutoMapperUtils : Profile

     
    {
        private IMapper mapper;
        private MapperConfiguration moviesConfig;

        #region Constructor
        public AutoMapperUtils()
        {
            moviesConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<JObject, Movies>().ConvertUsing<JObjectToMovieConverter>();
            });
        }
        #endregion
        public Movies MapJsonMovieToEntityMoviesModel(JObject JsonObject)
        {
            mapper = moviesConfig.CreateMapper();
            return mapper.Map<Movies>(JsonObject);
        }
    }

    public class JObjectToMovieConverter : ITypeConverter<JObject, Movies>
    {
        public Movies Convert(JObject source, Movies destination, ResolutionContext context)
        {
            var _movie = new Models.Entities.Movies();
            _movie.Info = new Info();
            _movie.Info.Directors = new List<Directors>();
            _movie.Info.Actors = new List<Actors>();
            _movie.Info.Genres = new List<Genres>();
            _movie.Title = source.SelectToken("title")?.ToString() ?? string.Empty;
            _movie.Year = source.SelectToken("year")?.ToString() ?? string.Empty;
            _movie.Info.ImageUrl = source.SelectToken("image_url")?.ToString() ?? string.Empty;
            _movie.Info.RunningTime = source.SelectToken("running_time_secs")?.ToString() ?? string.Empty;
            _movie.Info.Plot = source.SelectToken("info.plot")?.ToString() ?? string.Empty;
            _movie.Info.Rank = source.SelectToken("info.rank")?.ToString() ?? string.Empty;
            _movie.Info.Rating = (Decimal?)source.SelectToken("info.rating") ?? 0;
            
            foreach (JToken token in source.FindTokens("genres"))
            {
                foreach (JValue value in token)
                {
                    _movie.Info.Genres.Add(new Genres(value.ToString()));
                }
                    
            }
            foreach (JToken token in source.FindTokens("actors"))
            {
                foreach (JValue value in token)
                {
                    _movie.Info.Actors.Add(new Actors(value.ToString()));
                }
                
            }
            foreach (JToken token in source.FindTokens("directors"))
            {
                foreach (JValue value in token)
                {
                    _movie.Info.Directors.Add(new Directors(value.ToString()));
                }
            }
            
            return _movie;
        }
    }
    public static class JsonExtensions
    {
        public static List<JToken> FindTokens(this JToken containerToken, string name)
        {
            List<JToken> matches = new List<JToken>();
            FindTokens(containerToken, name, matches);
            return matches;
        }

        private static void FindTokens(JToken containerToken, string name, List<JToken> matches)
        {
            if (containerToken.Type == JTokenType.Object)
            {
                foreach (JProperty child in containerToken.Children<JProperty>())
                {
                    if (child.Name == name)
                    {
                        matches.Add(child.Value);
                    }
                    FindTokens(child.Value, name, matches);
                }
            }
            else if (containerToken.Type == JTokenType.Array)
            {
                foreach (JToken child in containerToken.Children())
                {
                    FindTokens(child, name, matches);
                }
            }
        }
    }
}
