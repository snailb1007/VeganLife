// <copyright file="StringProcessHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Helpers
{
    using System.Text;
    using System.Text.RegularExpressions;

    public static class StringProcessHelper
    {
        public static string ExtractImageSrc(string data)
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

        public static string GetHyperlink(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return string.Empty;
            }
            else
            {
                var startIndex = data.IndexOf("(");
                return data.Substring(startIndex + 1, data.IndexOf(")") - startIndex);
            }
        }

        public static List<BaseDataModel> ParseTextData(string text)
        {
            List<BaseDataModel> informations = new List<BaseDataModel>();

            // Split the text into sections based on the pattern
            string[] sections = Regex.Split(text, @"\*\*(?=\d+\.)");

            foreach (string section in sections)
            {
                if (!string.IsNullOrWhiteSpace(section))
                {
                    section = "**" + section;
                    // Extract title and content
                    string title = Regex.Match(section, @"(?<=\*\*).+?(?=\*\*)").Value.Trim();
                    string content = Regex.Replace(section, @"^.+?\*\*(.+)", "$1", RegexOptions.Singleline).Trim();

                    informations.Add(new BaseDataModel { Title = title, Description = content });
                }
            }

            return informations;
        }
    }
}
