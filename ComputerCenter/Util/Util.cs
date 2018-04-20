using System.Text.RegularExpressions;

namespace ComputerCenter.Util
{
    public static class Util
    {
        public static bool IsNumber(string text)
        {
            var regex = new Regex("[^0-9]+");
            return !regex.IsMatch(text);
        }

        public static bool IsNumberOrDot(string text)
        {
            var regex = new Regex("[^0-9.]+");
            return !regex.IsMatch(text);
        }
    }
}