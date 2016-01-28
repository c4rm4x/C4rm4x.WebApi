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
        public class AbstractSettingsManagerGetSettingAsTest
        {
            [TestMethod, UnitTest]
            [ExpectedException(typeof(ArgumentException))]
            public void GetSettingAs_Throws_Exception_When_No_Setting_Associated_With_Specified_Key_Exists()
            {
                GetSettingAs<string>(null);
            }

            [TestMethod, UnitTest]
            [ExpectedException(typeof(InvalidCastException))]
            public void GetSettingAs_Throws_Exception_When_Setting_Associated_With_Specified_Key_Exists_But_Is_Not_Type_Of()
            {
                GetSettingAs<int>(ObjectMother.Create<string>());
            }

            [TestMethod, UnitTest]
            public void GetSettingAs_Returns_Setting_Associated_With_Specified_Key_When_This_Exists_And_Is_Type_Of()
            {
                var Setting = ObjectMother.Create<string>();

                Assert.AreSame(
                    Setting,
                    GetSettingAs<string>(Setting));
            }

            private static TSetting GetSettingAs<TSetting>(object value)
            {
                return CreateSubjectUnderTest(value)
                    .GetSettingAs<TSetting>(Key);
            }
        }
    }
}
