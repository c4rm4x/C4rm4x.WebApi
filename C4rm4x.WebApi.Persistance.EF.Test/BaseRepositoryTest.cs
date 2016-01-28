#region Using

using C4rm4x.WebApi.Persistance.EF.Test.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace C4rm4x.WebApi.Persistance.EF.Test
{
    public partial class BaseRepositoryTest
    {
        private const string Value = "Value";

        [TestClass]
        public abstract class BaseRepositoryFixture :
            BasePersistanceFixture<TestTableRepository>
        {
            [TestInitialize]
            public override void Setup()
            {
                base.Setup();

                for (int i = 1; i <= 10; i++)
                    ExecuteNonSql(string.Format(Resource.InsertInto, i));
            }
        }
    }
}
