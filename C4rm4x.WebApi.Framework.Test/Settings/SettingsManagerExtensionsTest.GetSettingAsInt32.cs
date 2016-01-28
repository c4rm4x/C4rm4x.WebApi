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
        public class SettingsManagerExtensionGetSettingAsInt32Test
        {
            [TestMethod, UnitTest]
            public void GetSettingAsInt32_Returns_Setting_As_Int32()
            {
                var Returns = ObjectMother.Create<int>();

                var result = GetSettingAsInt32(Returns);

                Assert.IsNotNull(result);
                Assert.AreEqual(Returns, result);
            }

            [TestMethod, UnitTest]
            [ExpectedException(typeof(FormatException))]
            public void GetSettingAsInt32_Throws_FormatException_When_Setting_Is_Not_A_Valid_Int32_Representation()
            {
                GetSettingAsInt32(ObjectMother.Create<string>());
            }

            private static int GetSettingAsInt32(object returns)
            {
                return CreateSubjectUnderTest(returns)
                    .GetSettingAsInt32(Key);
            }
        }
    }
}
