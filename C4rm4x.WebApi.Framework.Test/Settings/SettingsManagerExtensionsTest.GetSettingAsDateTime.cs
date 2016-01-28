#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Framework.Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

#endregion

namespace C4rm4x.WebApi.Framework.Test.Settings
{
    public partial class SettingsManagerExtensionsTest
    {
        [TestClass]
        public class SettingsManagerExtensionGetSettingAsDateTimeTest
        {
            [TestMethod, UnitTest]
            public void GetSettingAsDateTime_Returns_Setting_As_DateTime()
            {
                const string DateFormat = "dd/MM/yyyy HH:mm:ss";

                var Returns = ObjectMother.Create<DateTime>();

                var result = GetSettingAsDateTime(Returns);

                Assert.IsNotNull(result);
                Assert.AreEqual(
                    Returns.ToString(DateFormat),
                    result.ToString(DateFormat));
            }

            [TestMethod, UnitTest]
            [ExpectedException(typeof(FormatException))]
            public void GetSettingAsDateTime_Throws_FormatException_When_Setting_Is_Not_A_Valid_DateTime_Representation()
            {
                GetSettingAsDateTime(ObjectMother.Create<string>());
            }

            private static DateTime GetSettingAsDateTime(object returns)
            {
                return CreateSubjectUnderTest(returns)
                    .GetSettingAsDateTime(Key);
            }
        }
    }
}
