using C4rm4x.WebApi.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace C4rm4x.WebApi.Events.EF.Configuration.Test
{
    public partial class EventStoreConfigurationBuilderTest
    {
        [TestClass]
        public abstract class EventStoreConfigurationBuilderFixture
        {
            protected EventStoreConfigurationBuilder _sut = EventStoreConfigurationBuilder.Configure();

            #region Helper classes

            public class TestApiEventData : ApiEventData
            {
                public TestApiEventData() : base()
                {
                }
            }

            #endregion
        }
    }
}