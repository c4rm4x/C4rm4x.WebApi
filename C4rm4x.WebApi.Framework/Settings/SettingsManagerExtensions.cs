#region Using

using System;
using System.Collections.Generic;

#endregion

namespace C4rm4x.WebApi.Framework.Settings
{
    /// <summary>
    /// Utilities methods to retrieve settings from SettingsManager based on types
    /// </summary>
    public static class SettingsManagerExtensions
    {
        /// <summary>
        /// Retrieves the setting associated with the specified key as a string
        /// </summary>
        /// <param name="settingsManager">The settings manager</param>
        /// <param name="key">The key</param>
        /// <returns>The setting as a string</returns>
        /// <exception cref="ArgumentException">Throws when no setting asssociated with specified key can be found</exception>
        public static string GetSettingAsString(
            this ISettingsManager settingsManager,
            object key)
        {
            return settingsManager.GetSetting(key).ToString();
        }

        /// <summary>
        /// Retrieves the setting associated with the specified key as a string
        /// or @default if none
        /// </summary>
        /// <param name="settingsManager">The settings manager</param>
        /// <param name="key">The key</param>
        /// <param name="default">The default value to return in case no setting is associated with the specified key</param>
        /// <returns>The setting as a string if exists; @default, otherwise</returns>
        public static string GetSettingAsString(
            this ISettingsManager settingsManager,
            object key,
            string @default)
        {
            try
            {
                return settingsManager.GetSettingAsString(key);
            }
            catch (ArgumentException)
            {
                return @default;
            }
        }

        /// <summary>
        /// Retrieves the setting associated with the specified key as an Int32
        /// </summary>
        /// <param name="settingsManager">The settings manager</param>
        /// <param name="key">The key</param>
        /// <returns>The setting as an Int32</returns>
        /// <exception cref="ArgumentException">Throws when no setting asssociated with specified key can be found</exception>
        /// <exception cref="FormatException">Throws when settings value is not in the correct format</exception>
        /// <exception cref="OverflowException">Throws when setting value represent a number less than System.Int32.MinValue or greater than System.Int32.MaxValue</exception>
        public static Int32 GetSettingAsInt32(
            this ISettingsManager settingsManager,
            object key)
        {
            return Int32.Parse(settingsManager.GetSettingAsString(key));
        }

        /// <summary>
        /// Retrieves the setting associated with the specified key as a DateTime
        /// </summary>
        /// <param name="settingsManager">The settings manager</param>
        /// <param name="key">The key</param>
        /// <returns>The setting as a DateTime</returns>
        /// <exception cref="ArgumentException">Throws when no setting asssociated with specified key can be found</exception>
        /// <exception cref="FormatException">Throws when setting value does not contain a valid string representation of a date and time</exception>
        public static DateTime GetSettingAsDateTime(
            this ISettingsManager settingsManager,
            object key)
        {
            return DateTime.Parse(settingsManager.GetSettingAsString(key));
        }

        /// <summary>
        /// Retrieves all the settings associated with the specified key as a collection of strings
        /// from a string of settings as "separator"-separated
        /// </summary>
        /// <param name="settingsManager">The settings manager</param>
        /// <param name="key">The key</param>
        /// <param name="separator">Character used to split of the settings</param>
        /// <param name="options">System.StringSplitOptions.RemoveEmptyEntries to omit empty array elements</param>
        /// from the array returned; or System.StringSplitOptions.None to include empty
        /// array elements in the array returned
        /// <returns>The list of all settings</returns>
        /// <exception cref="ArgumentException">Throws when no setting asssociated with specified key can be found</exception>
        public static IEnumerable<string> GetAllSettingsAsString(
            this ISettingsManager settingsManager,
            object key,
            char separator = ',',
            StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
        {
            return settingsManager
                .GetSettingAsString(key)
                .Split(new char[] { separator }, options);
        }
    }
}
