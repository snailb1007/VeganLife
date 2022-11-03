using System.Diagnostics;

namespace VeganLife.Helpers
{
    public enum UserSettingKey
    {
        SelectedTheme,
        ThemeMode,
        ImgBackground
    }

    public static partial class UserSettingsHelper
    {
        static readonly Dictionary<string, string> _cache = new Dictionary<string, string>();

        public static string Get(UserSettingKey key)
        {
            return Get(key.ToString());
        }

        public static void Set(UserSettingKey key, string value)
        {
            Set(key.ToString(), value);
        }

        public static bool Remove(string key)
        {
            _cache.Remove(key);
            return SecureStorage.Remove(key);
        }

        private static string Get(string key)
        {
            // ContainsKey need to be check or it will lead to KeyNotFoundException
            var value = _cache.ContainsKey(key) ? _cache[key] : null;
            if (value != null)
                return value;

            try
            {
                var task = Task.Run(async () =>
                {
                    value = await SecureStorage.GetAsync(key);
                });
                task.Wait();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            _cache[key] = value;
            return value;
        }

        private static void Set(string key, string value)
        {
            if (value == null)
            {
                Remove(key);
            }
            else
            {
                _cache[key] = value;
                try
                {
                    var task = Task.Run(async () => { await SecureStorage.SetAsync(key, value); });
                    task.Wait();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }
    }
}
