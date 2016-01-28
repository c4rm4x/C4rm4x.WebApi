#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.Settings;
using Moq;
using System;

#endregion

namespace C4rm4x.WebApi.Framework.Test.Settings
{
    public partial class SettingsManagerExtensionsTest
    {
        private const string Key = "key";

        private static ISettingsManager CreateSubjectUnderTest(
            object returns)
        {
            var mock = Mock.Of<ISettingsManager>();

            if (returns.IsNull())
                Mock.Get(mock)
                    .Setup(m => m.GetSetting(Key))
                    .Throws<ArgumentException>();
            else
                Mock.Get(mock)
                    .Setup(m => m.GetSetting(Key))
                    .Returns(returns);

            return mock;
        }
    }
}
