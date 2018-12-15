using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace C4rm4x.WebApi.Events.EF.Test
{
    public partial class EventStorePolicyTest    {        [TestClass]        public class EventStorePolicyIsSensitiveTest        {            #region Helper classes
            public class TestApiEventData : ApiEventData            {                public TestApiEventData() : base()                {
                }            }
            #endregion
            [TestMethod, UnitTest]            public void IsSensitive_Returns_True_When_Configuration_Has_Flagged_This_Type_As_Sensitive()            {                Setup(true);
                Assert.IsTrue(EventStorePolicy.IsSensitive(new TestApiEventData()));            }
            [TestMethod, UnitTest]            public void IsSensitive_Returns_False_When_Configuration_Has_Not_Flagged_This_Type_As_Sensitive()            {                Setup(false);
                Assert.IsFalse(EventStorePolicy.IsSensitive(new TestApiEventData()));            }
            [TestMethod, UnitTest]            public void IsSensitive_Returns_False_When_No_Configuration_Has_Been_Set()            {                Assert.IsFalse(EventStorePolicy.IsSensitive(new TestApiEventData()));            }
            private static void Setup(bool isSensitive = false)            {                var configuration = Mock.Of<IEventStoreConfiguration>();
                Mock.Get(configuration)                    .Setup(c => c.IsSensitive(It.IsAny<Type>()))                    .Returns(isSensitive);
                EventStorePolicy.SetConfiguration(configuration);            }        }    }
}
