using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace C4rm4x.WebApi.Events.EF.Configuration.Test
{
    public partial class EventStoreConfigurationBuilderTest
    {
        [TestClass]
        public class EventStoreConfigurationBuilderShouldIgnoreTest
            : EventStoreConfigurationBuilderFixture
        {
            [TestMethod, UnitTest]
            public void ShouldIgnore_Adds_Type_As_Type_To_Ignore()
            {
                var configuration = _sut
                    .ShouldIgnore<TestClass>()
                    .Build()
                    as EventStoreConfiguration;

                Assert.IsTrue(configuration.TypesToIgnore.Contains(typeof(TestClass)));
            }
        }
    }
}