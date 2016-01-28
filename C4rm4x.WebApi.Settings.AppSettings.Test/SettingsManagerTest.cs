#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace C4rm4x.WebApi.Settings.AppSettings.Test
{
    public partial class SettingsManagerTest
    {
        private const string Key = "SettingKey";

        [TestClass]
        public abstract class SettingsManagerFixture :
            AutoMockFixture<SettingsManager>
        { }
    }
}
