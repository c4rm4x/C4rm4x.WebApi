#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Framework.Persistance;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Persistance.EF.Test
{
    public partial class BaseRepositoryTest
    {
        [TestClass]
        public class BaseRepositoryExecuteQueryTest
            : BaseRepositoryFixture
        {
            [TestMethod, IntegrationTest]
            [ExpectedException(typeof(PersistenceException))]
            public void ExecuteQuery_Throws_PersistenceException_When_Any_Error_Occurs()
            {
                _sut.ExecuteQuery("non_existing_sp");
            }
        }
    }
}
