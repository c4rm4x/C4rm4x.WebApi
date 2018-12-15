using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace C4rm4x.WebApi.Events.EF.Configuration.Test
{
    public partial class EventStoreConfigurationTest    {        [TestClass]        public class EventStoreConfigurationSensitivePayloadTest :            EventStoreConfigurationFixture        {            [TestMethod, UnitTest]            public void SensitivePayload_Adds_New_Type_As_Sensitive_Event_Type()            {                var events = TryAddSensitivePayload<TestApiEventData>();
                Assert.IsTrue(events.Contains(typeof(TestApiEventData)));            }
            [TestMethod, UnitTest]            public void SensitivePayload_Does_Not_Add_Type_As_Sensitive_Event_Type_When_This_Already_Exists()            {                var events = TryAddSensitivePayload<TestApiEventData>();
                Assert.AreEqual(1, events.Count(type => type == typeof(TestApiEventData)));                events = TryAddSensitivePayload<TestApiEventData>();
                Assert.AreEqual(1, events.Count(type => type == typeof(TestApiEventData)));            }        }    }
}
