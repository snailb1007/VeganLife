// <copyright file="UserSettingsHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AndroidX.ConstraintLayout.Core;
using System.Collections.Concurrent;

namespace VeganLife.Helpers
{
    public enum UserSettingKey
    {
        SelectedTheme,
        HasPriorInstances,
        IsAcceptedCollectLogs,
        IsAcceptedTermsAndConditions,
        // IsDisplayedPolicyBox,
        // IsDisplayedLogsPermissionBox,
    }

    public static partial class UserSettingsHelper
    {
        private static readonly ConcurrentDictionary<string, string> _cache = new ConcurrentDictionary<string, string>();

        public static async Task<string> GetAsync(UserSettingKey key)
        {
            return await GetAsync(key.ToString());
        }

        public static async Task SetAsync(UserSettingKey key, string value)
        {
            await Set(key.ToString(), value);
        }

        public static bool Remove(string key)
        {
            _cache.TryRemove(key, out _);
            return SecureStorage.Remove(key);
        }

        private static async Task<string> GetAsync(string key)
        {
            if (!_cache.TryGetValue(key, out var value))
            {
                value = await SecureStorage.GetAsync(key);
                if (value != null)
                {
                    _cache[key] = value;
                }
            }

            return value!;
        }

        private static async Task Set(string key, string value)
        {
            if (value == null)
            {
                Remove(key);
            }
            else
            {
                if (_cache.TryAdd(key, value) || _cache[key] != value)
                {
                    _cache[key] = value;
                    try
                    {
                        await SecureStorage.SetAsync(key, value);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                        throw; // Consider rethrowing to notify about the failure
                    }
                }
            }
        }

        public static async Task<bool> GetBoolKey(UserSettingKey key)
        {
            var value = await GetAsync(key);
            return bool.TryParse(value, out bool result) && result;
        }
    }
}
