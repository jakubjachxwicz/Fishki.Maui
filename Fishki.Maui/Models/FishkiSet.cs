using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Fishki.Maui.Models
{
    public class FishkiSet
    {
        [JsonPropertyName("set_id")]
        public int SetId { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("lang_1")]
        public string FirstLanguage { get; set; }
        [JsonPropertyName("lang_2")]
        public string SecondLanguage { get; set; }
        [JsonPropertyName("words_count")]
        public int WordsCount { get; set; }
    }
}
