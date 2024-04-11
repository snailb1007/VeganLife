// <copyright file="UserSettingsHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Helpers
{
    public enum UserSettingKey
    {
        SelectedTheme,
        IsFirstTime,
        IsAcceptedCollectLogs,
        IsAcceptedTermsAndConditions,
        // IsDisplayedPolicyBox,
        // IsDisplayedLogsPermissionBox,
    }

    public static partial class UserSettingsHelper
    {
        public static bool IsFirstTime => string.IsNullOrEmpty(Get(UserSettingKey.IsFirstTime));

        public static bool IsAcceptedCollectLogs => GetBoolKey(UserSettingKey.IsAcceptedCollectLogs);

        public static bool IsAcceptedTermsAndConditions => GetBoolKey(UserSettingKey.IsAcceptedTermsAndConditions);
    }

    public static partial class UserSettingsHelper
    {
        static readonly Dictionary<string, string> cache = new Dictionary<string, string>();

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
            cache.Remove(key);
            return SecureStorage.Remove(key);
        }

        private static string Get(string key)
        {
            // ContainsKey need to be check or it will lead to KeyNotFoundException
            var value = cache.ContainsKey(key) ? cache[key] : null;
            if (value != null)
            {
                return value;
            }

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
                _ = ex;
#if DEBUG
                Debug.WriteLine(ex.Message);
#endif
            }

            cache[key] = value;
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
                cache[key] = value;
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

        private static bool GetBoolKey(UserSettingKey key)
        {
            var data = Get(key);
            return !string.IsNullOrEmpty(data) && Convert.ToBoolean(data);
        }
    }
}
