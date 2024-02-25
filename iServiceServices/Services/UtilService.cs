using System.Text.RegularExpressions;

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
