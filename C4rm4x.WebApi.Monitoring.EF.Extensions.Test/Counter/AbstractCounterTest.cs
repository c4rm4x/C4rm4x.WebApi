#region Using

using C4rm4x.WebApi.Framework.Persistance;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;

#endregion

namespace C4rm4x.WebApi.Monitoring.EF.Test
{
    public partial class AbstractCounterTest
    {
        #region Helper classes

        public class TestCounter : 
            AbstractCounter<TestTable, TestEntities>
        {
            public TestCounter(IRepository<TestTable> repository) : 
                base("testCounter", "testCounter", repository)
            {
            }
        }

        #endregion

        [TestClass]
        public abstract class AbstractCounterFixture :
            BasePersistanceFixture<TestCounter>
        {
            [TestInitialize]
            public override void Setup()
            {
                base.Setup();

                for (int i = 1; i <= 10; i++)
                    ExecuteNonSql(string.Format(Resource.InsertInto, i));
            }

            protected override void RegisterDependencies(
                Container container, 
                Lifestyle lifeStyle)
            {
                container.Register(typeof(IRepository<TestTable>), typeof(TestTableRepository), lifeStyle);

                base.RegisterDependencies(container, lifeStyle);
            }
        }
    }
}
