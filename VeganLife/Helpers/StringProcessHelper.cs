namespace VeganLife.Helpers
{
    public static class StringProcessHelper
    {
        public static string ExtractImgSrc(string data)
        {
            // Get the index of where the value of src starts.
            int start = data.IndexOf("<img src=\"") + 10;
            // Get the substring that starts at start, and goes up to first \".
            string src = data.Substring(start, data.IndexOf("\"", start) - start);
            return src;
        }
    }
}
