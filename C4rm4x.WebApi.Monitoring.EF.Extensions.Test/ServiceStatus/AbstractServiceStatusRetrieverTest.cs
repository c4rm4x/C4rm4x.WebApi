#region Using

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace C4rm4x.WebApi.Monitoring.EF.Test
{
    public partial class AbstractServiceStatusRetrieverTest
    {
        #region Helper classes

        public class TestServiceStatusRetriever :
            AbstractServiceStatusRetriever<TestEntities>
        {
            public TestServiceStatusRetriever(TestEntities entities) 
                : base("testServiceStatusRetriever", "testServiceStatusRetriever", entities)
            {
            }
        }

        #endregion

        [TestClass]
        public abstract class AbstractServiceStatusRetrieverFixture :
            BasePersistanceFixture<TestServiceStatusRetriever>
        {
        }
    }
}
