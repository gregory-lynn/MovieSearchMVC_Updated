using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MvcMovie.Models.Entities
{
    public class Movies : EntityBaseModel
    {
        [JsonProperty("year")]
        public string Year { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("info")]
        public Info Info { get; set; }
    }
}