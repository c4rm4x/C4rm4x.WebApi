#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Framework.Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace C4rm4x.WebApi.Framework.Test.Settings
{
    public partial class SettingsManagerExtensionsTest
    {
        [TestClass]
        public class SettingsManagerExtensionGetSettingAsStringTest
        {
            [TestMethod, UnitTest]
            public void GetSettingAsString_Returns_Setting_As_String()
            {
                var Returns = ObjectMother.Create<string>();

                var result = GetSettingAsString(Returns);

                Assert.IsNotNull(result);
                Assert.AreEqual(Returns, result);
            }

            [TestMethod, UnitTest]
            public void GetSettingsAsString_Returns_Default_Value_When_ArgumentException_Is_Thrown()
            {
                var Default = ObjectMother.Create<string>();

                var result = GetSettingAsString(null, Default);

                Assert.IsNotNull(result);
                Assert.AreEqual(Default, result);
            }

            private static string GetSettingAsString(
                object returns,
                string @default = "")
            {
                return CreateSubjectUnderTest(returns)
                    .GetSettingAsString(Key, @default);
            }
        }
    }
}
