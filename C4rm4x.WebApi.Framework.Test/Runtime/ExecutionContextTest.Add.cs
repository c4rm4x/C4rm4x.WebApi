#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

#endregion

namespace C4rm4x.WebApi.Framework.Test.Runtime
{
    public partial class ExecutionContextTest
    {
        [TestClass]
        public class ExecutionContextAddTest :
            ExecutionContextFixture
        {
            [TestMethod, UnitTest]
            public void Add_Adds_A_New_ExecutionContextExtension()
            {
                var extension = new TestExtension();

                _sut.Add(extension);

                Assert.IsTrue(_sut.Extensions.Any());
                Assert.AreEqual(1, _sut.Extensions.Count);
                Assert.AreSame(extension, _sut.Extensions.First());
            }

            [TestMethod, UnitTest]
            [ExpectedException(typeof(ArgumentException))]
            public void Add_Throws_Exception_When_Previous_ExecutionContextExtension_Of_Same_Type_Already_Exists()
            {
                _sut.Add(new TestExtension());
                _sut.Add(new TestExtension());
            }
        }
    }
}
