using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcMovie.Models.Entities
{
    public class Actors : EntityBaseModel
    {
        public string Actor { get; set; }
        [ForeignKey("Info")]
        public int InfoId { get; set; }
    }
}