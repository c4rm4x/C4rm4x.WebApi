#region Using

using C4rm4x.WebApi.Framework;
using C4rm4x.WebApi.Framework.Settings;
using System.Configuration;

#endregion

namespace C4rm4x.WebApi.Settings.AppSettings
{
    /// <summary>
    /// Implementation of ISettingsManager using ConfigurationManager.AppSettings
    /// </summary>
    [DomainService(typeof(ISettingsManager))]
    public class SettingsManager :
        AbstractSettingsManager, ISettingsManager
    {
        /// <summary>
        /// Retrieves the setting from ConfigurationManager.AppSettings based on key
        /// </summary>
        /// <typeparam name="Tkey">Type of the key</typeparam>
        /// <param name="key">The key that identifies the settings</param>
        /// <returns>The settings associated to the key in the ConfigurationManager.AppSettings</returns>
        protected override object RetrieveSettingBy<Tkey>(Tkey key)
        {
            return ConfigurationManager.AppSettings[key.ToString()];
        }
    }
}
