#region Using

using C4rm4x.WebApi.Framework.Settings;

#endregion

namespace C4rm4x.WebApi.Framework.Test.Settings
{
    public partial class AbstractSettingsManagerTest
    {
        private const string Key = "key";

        #region Helper classes

        private class TestSettingsManager : AbstractSettingsManager
        {
            public object Setting { get; private set; }

            public TestSettingsManager(object setting)
            {
                Setting = setting;
            }

            protected override object RetrieveSettingBy<Tkey>(Tkey key)
            {
                return Setting;
            }
        }

        #endregion

        private static ISettingsManager CreateSubjectUnderTest(object value)
        {
            return new TestSettingsManager(value);
        }
    }
}
