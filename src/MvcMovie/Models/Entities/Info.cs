using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MvcMovie.Models.Entities
{
    public class Info : EntityBaseModel
    {
        [ForeignKey("Movie")]
        public int MovieId { get; set; }
        [JsonProperty("directors")]
        public List<Directors> Directors { get; set; }
        [DataType(DataType.Date), JsonProperty("release_date")]
        public DateTime ReleaseDate { get; set; }
        [JsonProperty("rating"), AllowNull]
        public decimal Rating { get; set; }
        [JsonProperty("genres")]
        public List<Genres> Genres { get; set; }
        [JsonProperty("image_url")]
        public string ImageUrl { get; set; }
        [JsonProperty("plot")]
        public string Plot { get; set; }
        [JsonProperty("rank")]
        public string Rank { get; set; }
        [JsonProperty("running_time_secs"), AllowNull]
        public string RunningTime { get; set; }
        [JsonProperty("actors")]
        public List<Actors> Actors { get; set; }
    }
}