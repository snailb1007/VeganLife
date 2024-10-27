using System.Reflection;
using VeganLife.Helpers.Extensions;

namespace VeganLife.Helpers
{
    public static class ResourceReader
    {
        public static async Task<string> ReadTextFileAsync(string resourceName)
        {
            string result = string.Empty;
            var assembly = Assembly.GetExecutingAssembly();
            if (assembly is null)
            {
                return result;
            }

            Stream? stream = assembly.GetManifestResourceStream(resourceName);
            try
            {
                if (stream is not null)
                {
                    using (var reader = new StreamReader(stream))
                    {
                        result = await reader.ReadToEndAsync();
                    }
                }

                return result;
            }
            catch (Exception)
            {
                return result;
            }
            finally
            {
                stream?.Dispose();
            }
        }

        public static Color GetResourceColorByKey(string key)
        {
            var targetColor = GetResourceByKey(key) as AppThemeColor;
            if (targetColor is null)
            {
                return Colors.Transparent;
            }

            var currentTheme = Application.Current?.RequestedTheme;
            var result = targetColor.Default ?? Colors.Transparent;
            switch (currentTheme)
            {
                case AppTheme.Light:
                    if (targetColor.Light is not null)
                    {
                        result = targetColor.Light;
                    }

                    break;
                case AppTheme.Dark:
                    if (targetColor.Dark is not null)
                    {
                        result = targetColor.Dark;
                    }

                    break;
            }

            return result;

            // TODO: https://github.com/dotnet/maui/pull/11214
            object GetResourceByKey(string key)
            {
                if (!string.IsNullOrEmpty(key) && Application.Current is not null && Application.Current.Resources.TryGetValue(key, out object resource))
                {
                    return resource;
                }
                else
                {
                    UtilitiesExtension.LogError($"{nameof(GetResourceByKey)}: {key} not found!");
                    return null;
                }
            }
        }
    }
}
