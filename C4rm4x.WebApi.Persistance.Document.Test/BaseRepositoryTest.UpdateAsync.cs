#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Persistance.Document.Test
{
    public partial class BaseRepositoryTest
    {
        [TestClass]
        public class BaseRepositoryUpdateAsyncTest :
            BaseRepositoryFixture
        {
            [TestMethod, IntegrationTest]
            public void UpdateAsync_Replaces_Entity_In_Context()
            {
                var NewValue = ObjectMother.Create(10);

                var entity = GetEntityByValue("1");

                Assert.IsNotNull(entity);

                entity.Value = NewValue;
                
                _sut.UpdateAsync(entity).Wait(); // For testing

                Assert.IsNotNull(GetEntityByValue(NewValue));
            }
        }
    }
}
