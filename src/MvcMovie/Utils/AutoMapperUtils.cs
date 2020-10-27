using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MvcMovie.Models.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Reflection;

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
        static readonly IContractResolver defaultResolver = JsonSerializer.CreateDefault().ContractResolver;

        public Movies Convert(JObject source, Movies destination, ResolutionContext context)
        {
            // This is the entry point for converting from JObject to Movies
            // method not directly referenced, it is used in AutoMapperUtils constructor => ConvertUsing 
            destination = new Movies();
            destination = (Movies)SetPropertyValues(source, destination);
            return destination;
        }

        private Object SetPropertyValues(JObject source, Object obj)
        {
            // use reflection to set values from JSON object matching by JsonProperty name
            foreach (PropertyInfo prop in obj.GetType().GetProperties())
            {
                var JsonPropertyName = JsonExtensions.GetJsonPropertyName(defaultResolver, prop.Name, obj.GetType());
                var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                if (!string.IsNullOrEmpty(JsonPropertyName) && type != null)
                {
                    if (type == typeof(string))
                    {
                        foreach (JValue value in source.FindTokens(JsonPropertyName)){
                            prop.SetValue(obj, value?.ToString() ?? string.Empty); break;}
                    }
                    else if (type == typeof(DateTime))
                    {
                        foreach (JValue value in source.FindTokens(JsonPropertyName)){
                            prop.SetValue(obj, System.Convert.ToDateTime(
                                value?.ToString() ?? DateTime.MinValue.ToString())); break;}
                    }
                    else if (type == typeof(Decimal))
                    {
                        foreach (JValue value in source.FindTokens(JsonPropertyName)){
                            prop.SetValue(obj, System.Convert.ToDecimal(value?.ToString() ?? "0.0")); break;}
                    }
                    else if (type == typeof(Info))
                    {
                        // create a new instance of the Info object
                        // this is the only place where I used static classes instead of generics/reflection
                        // Todo: can this be changed to somehow use reflection?
                        // perhaps incorporate overload method functionality in this one using reflection to get types
                        Info info = (Info)Activator.CreateInstance(type);
                        info.Actors = (List<Actors>)SetPropertyValues(source, info.Actors, type);
                        info.Directors = (List<Directors>)SetPropertyValues(source, info.Directors, type);
                        info.Genres = (List<Genres>)SetPropertyValues(source, info.Genres, type);
                        info = (Info)SetPropertyValues(source, info);
                        prop.SetValue(obj, info);
                    }
                }
            }
            return obj;
        }

        private List<T> SetPropertyValues<T>(JObject source, List<T> objList, Type parentType)
        {
            Type typeParameterType = typeof(T);
            var JsonPropertyName = JsonExtensions.GetJsonPropertyName(defaultResolver, typeParameterType.Name, parentType);

            foreach (JToken token in source.FindTokens(JsonPropertyName)) 
            {
                foreach (JValue value in token)
                {
                    // create a new object instance based on type of generic object list from param
                    Object instance =
                    (Object)System.ComponentModel.TypeDescriptor.CreateInstance(
                        provider: null, // use standard type description provider, which uses reflection
                        objectType: typeParameterType,
                        argTypes: new Type[] { typeof(string) },
                        args: new object[] { value.ToString() } // actors, directors, genres each have string param in constructor
                    );
                    objList.Add((T)instance);
                }
            }
            return objList;
        }
    }
    public static class JsonExtensions
    {
        static readonly IContractResolver defaultResolver = JsonSerializer.CreateDefault().ContractResolver;

        public static List<JToken> FindTokens(this JToken containerToken, string name)
        {
            // recursively traverse JToken to find values for input string
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
                    FindTokens(child.Value, name, matches); // recursive call
                }
            }
            else if (containerToken.Type == JTokenType.Array)
            {
                foreach (JToken child in containerToken.Children())
                {
                    FindTokens(child, name, matches); // recursive call
                }
            }
        }

        public static string GetJsonPropertyName(this IContractResolver resolver, string propertyName, Type type)
        {
            // Get the JsonProperty value by Model Property Name
            if (resolver == null || type == null)
                throw new ArgumentNullException();
            var contract = resolver.ResolveContract(type) as JsonObjectContract;
            if (contract == null)
                return string.Empty;
            return contract.Properties.Where(p => !p.Ignored && p.UnderlyingName.Equals(propertyName)).Select(p => p.PropertyName).FirstOrDefault().ToString();
        }
    }
}
