using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcMovie.Models.Entities
{
    public class Actors : EntityBaseModel
    {
        [JsonProperty("actor")]
        public string Actor { get; set; }
        [ForeignKey("Info")]
        public int InfoId { get; set; }
        public Actors(string actor)
        {
            Actor = actor;
        }
    }
}