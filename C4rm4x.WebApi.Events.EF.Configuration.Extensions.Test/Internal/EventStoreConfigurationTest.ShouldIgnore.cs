using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C4rm4x.WebApi.Events.EF.Configuration.Test
{
    public partial class EventStoreConfigurationTest
    {
        [TestClass]
        public class EventStoreConfigurationShouldIgnoreTest :
            EventStoreConfigurationFixture
        {
            [TestMethod, UnitTest]
            public void ShouldIgnore_Adds_New_Type_As_Type_To_Ignore()
            {
                var types = TryShouldIgnore<TestClass>();

                Assert.IsTrue(types.Contains(typeof(TestClass)));
            }

            [TestMethod, UnitTest]
            public void ShouldIgnore_Does_Not_Add_Type_As_Type_To_Ignore_When_This_Already_Exists()
            {
                var types = TryShouldIgnore<TestClass>();

                Assert.AreEqual(1, types.Count(type => type == typeof(TestClass)));

                types = TryShouldIgnore<TestClass>();

                Assert.AreEqual(1, types.Count(type => type == typeof(TestClass)));
            }

            [TestMethod, UnitTest]
            [ExpectedException(typeof(ArgumentException))]
            public void ShouldIgnore_Throws_Exception_When_Type_To_Ignore_Is_ApiEventData()
            {
                TryShouldIgnore<TestApiEventData>();
            }
        }
    }
}
