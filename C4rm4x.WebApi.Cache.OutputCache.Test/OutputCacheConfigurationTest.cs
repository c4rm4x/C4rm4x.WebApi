#region Using

using C4rm4x.WebApi.Framework.Cache;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Web.Http;

#endregion

namespace C4rm4x.WebApi.Cache.OutputCache.Test
{
    public partial class OutputCacheConfigurationTest
    {
        [TestClass]
        public abstract class OutputCacheConfigurationFixture
        {
            protected static ICache Cache = Mock.Of<ICache>();

            protected static OutputCacheConfiguration CreateSubjectUnderTest(
                HttpConfiguration config)
            {
                return config.GetOutputCacheConfiguration();
            }
        }
    }
}
