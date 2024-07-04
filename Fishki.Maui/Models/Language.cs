using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fishki.Maui.Models
{
    public class Language
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        // public string FlagIconUri { get => $"flag_{Code}.png"; set { } }
    }
}
