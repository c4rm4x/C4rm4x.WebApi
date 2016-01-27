#region Using

using C4rm4x.Tools.Utilities;
using System;

#endregion

namespace C4rm4x.WebApi.Framework.Settings
{
    /// <summary>
    /// Base class that implements ISettingsManager
    /// </summary>
    public abstract class AbstractSettingsManager :
        ISettingsManager
    {
        /// <summary>
        /// Retrieves a setting associated with the key 
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>The setting if it is present</returns>
        /// <exception cref="ArgumentException">Throws when either the key is null or a setting associated with the specified key cannot be found</exception>
        public object GetSetting(object key)
        {
            key.NotNull(nameof(key));

            var setting = RetrieveSettingBy(key);

            if (setting.IsNull())
                throw new ArgumentException(
                    "There is no setting associated with key {0}"
                        .AsFormat(key.ToString()));

            return setting;
        }

        /// <summary>
        /// Retrieves the setting based on specified key
        /// </summary>
        /// <typeparam name="Tkey">Type of key</typeparam>
        /// <param name="key">The key</param>
        /// <returns>The related setting (if any)</returns>
        protected abstract object RetrieveSettingBy<Tkey>(Tkey key);

        /// <summary>
        /// Retrieves a setting of type TSetting associated with the key 
        /// </summary>
        /// <typeparam name="TSetting">Type of the setting</typeparam>
        /// <param name="key">The key</param>
        /// <returns>The setting if it is present</returns>
        /// <exception cref="ArgumentException">Throws when either the key is null or a setting associated with the specified key cannot be found</exception>
        /// <exception cref="InvalidCastException">When setting cannot be cast to specified type</exception>
        public TSetting GetSettingAs<TSetting>(object key)
        {
            return (TSetting)GetSetting(key);
        }
    }
}
