using System.Text;

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

        public static string ConvertStringToUnSigned(this string s)
        {
            string stFormD = s.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }

            sb = sb.Replace('Đ', 'D');
            sb = sb.Replace('đ', 'd');
            return sb.ToString().Normalize(NormalizationForm.FormD).ToLower();
        }
    }
}
