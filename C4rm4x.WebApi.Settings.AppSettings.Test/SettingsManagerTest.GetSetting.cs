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
        public class SettingsManagerGetSettingTest :
            SettingsManagerFixture
        {
            [TestMethod, UnitTest]
            [ExpectedException(typeof(ArgumentException))]
            public void GetSetting_Throws_Exception_When_Setting_With_Specified_Key_Is_Not_Present_In_Configuration()
            {
                _sut.GetSetting(ObjectMother.Create<string>());
            }

            [TestMethod, UnitTest]
            public void GetSetting_Returns_Setting_With_Specified_Key_When_Is_Present_In_Configuration()
            {
                Assert.AreEqual(
                    ConfigurationManager.AppSettings[Key],
                    _sut.GetSetting(Key));
            }
        }
    }
}
