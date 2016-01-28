#region Using

using C4rm4x.WebApi.Persistance.EF.Test.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace C4rm4x.WebApi.Persistance.EF.Test
{
    public partial class UnitOfWorkTest
    {
        private const string Value = "Value";

        [TestClass]
        public abstract class UnitOfWorkFixture
            : BasePersistanceFixture<TestUnitOfWork>
        { }
    }
}
