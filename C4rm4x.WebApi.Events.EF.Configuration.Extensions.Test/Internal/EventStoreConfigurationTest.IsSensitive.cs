using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace C4rm4x.WebApi.Events.EF.Configuration.Test
{
    public partial class EventStoreConfigurationTest
    {
        [TestClass]
        public class EventStoreConfigurationIsSensitiveTest :
            EventStoreConfigurationFixture
        {
            [TestMethod, UnitTest]
            public void IsSensitive_Returns_False_When_The_Event_Type_Has_Not_Been_Flagged_As_Sensitive_Yet()
            {
                Assert.IsFalse(_sut.IsSensitive(new TestApiEventData()));
            }

            [TestMethod, UnitTest]
            public void IsSensitive_Returns_True_When_The_Event_Type_Has_Been_Already_Flagged_As_Sensitive()
            {
                TryAddSensitivePayload<TestApiEventData>();

                Assert.IsTrue(_sut.IsSensitive(new TestApiEventData()));
            }
        }
    }
}