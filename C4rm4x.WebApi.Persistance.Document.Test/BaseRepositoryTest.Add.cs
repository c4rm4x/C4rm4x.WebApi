#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Persistance.Document.Test.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace C4rm4x.WebApi.Persistance.Document.Test
{
    public partial class BaseRepositoryTest
    {
        [TestClass]
        public class BaseRepositoryAddTest :
            BaseRepositoryFixture
        {
            [TestMethod, IntegrationTest]
            public void Add_Adds_New_Entity_In_Context()
            {
                var entity = new TestEntity(Value);

                _sut.Add(entity);

                Assert.IsFalse(entity.Id.IsNullOrEmpty());
            }
        }
    }
}
