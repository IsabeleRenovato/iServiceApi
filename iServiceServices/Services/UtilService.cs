using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace iServiceServices.Services
{
    public class UtilService
    {
        public static string CleanString(string text)
        {
            string pattern = "[^a-zA-Z0-9\\s]";

            Regex regex = new Regex(pattern);

            string cleanedText = regex.Replace(text, "");

            return cleanedText;
        }
    }
}
