﻿using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcMovie.Models.Entities
{
    public class Directors : EntityBaseModel
    {
        [JsonProperty("director")]
        public string Director { get; set; }
        [ForeignKey("Info")]
        public int InfoId { get; set; }
        public Directors(string director)
        {
            Director = director;
        }
    }
}