using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace C4rm4x.WebApi.Events.EF.Configuration.Test
{
    public partial class EventStoreConfigurationBuilderTest
    {        [TestClass]        public class EventStoreConfigurationBuilderBuildTest : EventStoreConfigurationBuilderFixture        {            [TestMethod, UnitTest]            public void Build_Returns_A_New_Instance_Of_IEventStoreConfiguration()            {                var configuration = _sut.Build();
                Assert.IsNotNull(configuration);                Assert.IsInstanceOfType(configuration, typeof(IEventStoreConfiguration));            }        }    }
}
