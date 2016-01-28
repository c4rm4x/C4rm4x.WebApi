#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;

#endregion

namespace C4rm4x.WebApi.Settings.AppSettings.Test
{
    public partial class SettingsManagerTest
    {
        [TestClass]
        public class SettingsManagerGetSettingAsTest :
            SettingsManagerFixture
        {
            [TestMethod, UnitTest]
            [ExpectedException(typeof(ArgumentException))]
            public void GetSettingAs_Throws_Exception_When_Setting_With_Specified_Key_Is_Not_Present_In_Configuration()
            {
                _sut.GetSettingAs<string>(ObjectMother.Create<string>());
            }

            [TestMethod, UnitTest]
            public void GetSettingAs_Returns_Setting_With_Specified_Key_When_Is_Present_In_Configuration_And_Type_Is_String()
            {
                Assert.AreEqual(
                    ConfigurationManager.AppSettings[Key],
                    _sut.GetSettingAs<string>(Key));
            }

            [TestMethod, UnitTest]
            [ExpectedException(typeof(InvalidCastException))]
            public void GetSettingAs_Throws_Exception_When_Setting_With_Specified_Key_When_Is_Present_In_Configuration_And_But_Type_Is_Not_String()
            {
                _sut.GetSettingAs<int>(Key);
            }
        }
    }
}
