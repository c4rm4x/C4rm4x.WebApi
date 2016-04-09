#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Security.Acl.Subscriptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Net.Http;
using System.Web.Http;

#endregion

namespace C4rm4x.WebApi.Security.Acl.Test
{
    public partial class AclConfigurationTest
    {
        [TestClass]
        public class AclConfigurationGetSubscriberRepositoryTest :
            AclConfigurationFixture
        {
            [TestMethod, UnitTest]
            public void GetSubscriberRepository_Returns_Instance_From_Request_Dependency_Scope()
            {
                var SubscriberRepository = Mock.Of<ISubscriberRepository>();

                var sut = CreateSubjectUnderTest(new HttpConfiguration());

                sut.SetResolverFactory((requet, type) => SubscriberRepository);

                Assert.AreSame(
                    SubscriberRepository,
                    sut.GetSubscriberRepository(new HttpRequestMessage()));
            }
        }
    }
}
