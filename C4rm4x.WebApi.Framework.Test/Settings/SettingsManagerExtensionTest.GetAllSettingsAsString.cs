#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Framework.Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace C4rm4x.WebApi.Framework.Test.Settings
{
    public partial class SettingsManagerExtensionsTest
    {
        [TestClass]
        public class SettingsManagerExtensionsGetAllSettingsAsStringTest
        {
            [TestMethod, UnitTest]
            public void GetAllSettingsAsString_Returns_As_Many_Settings_As_Present_Separated_By_Comma()
            {
                var Returns = GetSettings(GetRand(10)).ToArray();
                var result = GetAllSettingsAsString(Returns);

                Assert.IsNotNull(result);
                Assert.AreEqual(Returns.Length, result.Count());

                for (int i = 0; i < Returns.Length; i++)
                    Assert.AreEqual(Returns[i], result.ElementAt(i));
            }

            private static int GetRand(int max)
            {
                return new Random().Next(1, max);
            }

            private static IEnumerable<string> GetSettings(int numberOfSettings)
            {
                for (int i = 0; i < numberOfSettings; i++)
                    yield return ObjectMother.Create<string>();
            }

            private static IEnumerable<string> GetAllSettingsAsString(
                IEnumerable<string> returns,
                char separator = ',',
                StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
            {
                return CreateSubjectUnderTest(
                    string.Join(separator.ToString(), returns))
                    .GetAllSettingsAsString(Key, separator, options);
            }
        }
    }
}
