using System.Reflection;

namespace VeganLife.Helpers
{
    public static class ResourceReader
    {
        public static async Task<string> ReadTextFileAsync(string resourceName)
        {
            string result = string.Empty;
            var assembly = Assembly.GetExecutingAssembly();
            if (assembly is null)
                return result;
            Stream? stream = assembly.GetManifestResourceStream(resourceName);
            try
            {
                if (stream is  not null)
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
    }
}
