#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

#endregion

namespace C4rm4x.WebApi.Framework.Test.Settings
{
    public partial class AbstractSettingsManagerTest
    {
        [TestClass]
        public class AbstractSettingsManagerGetSettingTest
        {
            [TestMethod, UnitTest]
            [ExpectedException(typeof(ArgumentException))]
            public void GetSetting_Throws_Exception_When_No_Setting_Associated_With_Specified_Key_Exists()
            {
                GetSetting(null);
            }

            [TestMethod, UnitTest]
            public void GetSetting_Returns_Setting_Associated_With_Specified_Key_When_This_Exists()
            {
                var Setting = ObjectMother.Create<string>();

                Assert.AreSame(
                    Setting,
                    GetSetting(Setting));
            }

            private static object GetSetting(object value)
            {
                return CreateSubjectUnderTest(value)
                    .GetSetting(Key);
            }
        }
    }
}
