﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace TomatoBot.Model.Moovies
{
    public class MoovieCollection
    {
        public int Page { get; set; }

        public List<Moovie> Results { get; set; }

        public Dates Dates { get; set; }

        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }

        [JsonProperty("total_results")]
        public int TotalResults { get; set; }
    }

}