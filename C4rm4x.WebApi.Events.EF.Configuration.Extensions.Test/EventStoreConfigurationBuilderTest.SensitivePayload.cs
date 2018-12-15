using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace C4rm4x.WebApi.Events.EF.Configuration.Test
{
    public partial class EventStoreConfigurationBuilderTest
    {
        [TestClass]
        public class EventStoreConfigurationBuilderSensitivePayloadTest
            : EventStoreConfigurationBuilderFixture
        {
            [TestMethod, UnitTest]
            public void SensitivePayload_Adds_Event_Type_As_Sensitive_Event_Data_Type()
            {
                var configuration = _sut
                    .SensitivePayload<TestApiEventData>()
                    .Build()
                    as EventStoreConfiguration;

                Assert.IsTrue(configuration.SensitiveEvents.Contains(typeof(TestApiEventData)));
            }
        }
    }
}