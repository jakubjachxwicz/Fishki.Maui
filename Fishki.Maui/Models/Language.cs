using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fishki.Maui.Models
{
    class Language
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string FlagIconUri { get => $"flag_{Code}.png"; }
    }
}
