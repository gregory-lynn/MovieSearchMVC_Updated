using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace MvcMovie.Models
{
    public class MoviesEntityViewModel
    {
        public List<Entities.Movies> Movies { get; set;}
        public bool isMoviesWithImagesOnly { get; set; }
        public string SearchString { get; set; }
    }
}