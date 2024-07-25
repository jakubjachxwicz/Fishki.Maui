using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Fishki.Maui.Utils
{
    public static class StaticMethods
    {
        public static bool IsPasswordValid(string password)
        {
            if (string.IsNullOrEmpty(password)) return false;

            Regex[] regexes = 
            [
                new Regex(@"[0-9]+"), 
                new Regex(@"[A-Z]+"),
                new Regex(@".{8,20}"),
                new Regex(@"[a-z]+")
            ];

            foreach (Regex regex in regexes)
                if (!regex.IsMatch(password)) return false;

            return true;
        }
    }
}
