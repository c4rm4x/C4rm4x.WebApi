#region Using

using System;

#endregion

namespace C4rm4x.WebApi.Framework.Settings
{
    /// <summary>
    /// Service responsible to retrieve settings from the specific storage
    /// </summary>
    public interface ISettingsManager
    {
        /// <summary>
        /// Retrieves a setting associated with the key 
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>The setting if it is present</returns>
        /// <exception cref="ArgumentException">Throws when either the key is null or a setting associated with the specified key cannot be found</exception>
        object GetSetting(object key);

        /// <summary>
        /// Retrieves a setting of type TSetting associated with the key 
        /// </summary>
        /// <typeparam name="TSetting">Type of the setting</typeparam>
        /// <param name="key">The key</param>
        /// <returns>The setting if it is present</returns>
        /// <exception cref="ArgumentException">Throws when either the key is null or a setting associated with the specified key cannot be found</exception>
        /// <exception cref="InvalidCastException">When setting cannot be cast to specified type</exception>
        TSetting GetSettingAs<TSetting>(object key);
    }
}
