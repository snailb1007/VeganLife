// <copyright file="StringProcessHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Text;
using System.Text.RegularExpressions;

namespace VeganLife.Helpers
{
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

        public static string GetHyperLink(string data)
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

        public static List<BaseDataModel> ParseSections(string text)
        {
            List<BaseDataModel> informations = new List<BaseDataModel>();

            // Adjusted regex pattern to capture title and description
            string pattern = @"(\d+)\.\s*(.*?)\n(.*?)\n?(?=\d+\.|$)";

            MatchCollection matches = Regex.Matches(text, pattern, RegexOptions.Singleline);

            foreach (Match match in matches)
            {
                string title = match.Groups[2].Value.Trim();
                string description = match.Groups[3].Value.Trim();
                if (title.Contains("**"))
                {
                    title = title.Replace("**", string.Empty);
                }

                if (description.Contains("**"))
                {
                    description = description.Replace("**", string.Empty);
                }

                informations.Add(new BaseDataModel { Title = title, Description = description });
            }

            return informations;
        }

        public static string GetNameContainVietnameseTranslations(string enName)
        {
            if (string.IsNullOrEmpty(enName))
            {
                return string.Empty;
            }

            // Try to get the Vietnamese translation
            foreach (var entry in NutritionFactsHelper.VietnameseTranslations)
            {
                if (enName.ToLower().Contains(entry.Key.ToLower(), StringComparison.OrdinalIgnoreCase))
                {
                    return entry.Value;
                }
            }

            // If no translation is available, just return the English name
            return enName;
        }

        public static string NormalizeString(this string input)
        {
            return input.Normalize(NormalizationForm.FormKD).ToLower().Trim();
        }

        public static IEnumerable<string> NormalizeStringAndSplit(this string input)
        {
            return input.Normalize(NormalizationForm.FormKD)
                .ToLower()
                .Split(',')
                .Select(s => s.Trim())
                .Where(s => !string.IsNullOrEmpty(s) && !string.IsNullOrWhiteSpace(s));
        }
    }
}