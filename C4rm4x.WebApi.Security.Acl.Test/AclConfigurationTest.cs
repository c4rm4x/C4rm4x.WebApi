#region Using

using C4rm4x.WebApi.Framework.Cache;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Web.Http;

#endregion

namespace C4rm4x.WebApi.Security.Acl.Test
{
    public partial class AclConfigurationTest
    {
        [TestClass]
        public abstract class AclConfigurationFixture
        {
            protected static ICache Cache = Mock.Of<ICache>();

            protected static AclConfiguration CreateSubjectUnderTest(
                HttpConfiguration config)
            {
                return config.GetAclConfiguration();
            }
        }
    }
}
