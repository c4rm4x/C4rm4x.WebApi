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
        public class ExecutionContextAddExtensionTest :
            ExecutionContextFixture
        {
            [TestMethod, UnitTest]
            public void AddExtension_Adds_A_New_ExecutionContextExtension()
            {
                var Extension = new TestExecutionContextExtension();

                _sut.AddExtension(Extension);

                Assert.IsTrue(_sut.Extensions.Any());
                Assert.AreEqual(1, _sut.Extensions.Count);
                Assert.AreSame(Extension, _sut.Extensions.First());
            }

            [TestMethod, UnitTest]
            [ExpectedException(typeof(ArgumentException))]
            public void AddExtension_Throws_Exception_When_Previous_ExecutionContextExtension_Of_Same_Type_Already_Exists()
            {
                _sut.AddExtension(new TestExecutionContextExtension());
                _sut.AddExtension(new TestExecutionContextExtension());
            }
        }
    }
}
