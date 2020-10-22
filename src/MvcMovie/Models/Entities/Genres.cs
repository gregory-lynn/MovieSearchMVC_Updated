using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcMovie.Models.Entities
{
    public class Genres : EntityBaseModel
    {
        public string Genre { get; set; }
        [ForeignKey("Info")]
        public int InfoId { get; set; }
    }
}