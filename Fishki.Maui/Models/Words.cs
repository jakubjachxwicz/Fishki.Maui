using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Fishki.Maui.Models
{
    public class Words
    {
        [JsonPropertyName("words_id")]
        public int WordsId { get; set; }
        [JsonPropertyName("word_1")]
        public string First { get; set; }
        [JsonPropertyName("word_2")]
        public string Second { get; set; }
    }
}
