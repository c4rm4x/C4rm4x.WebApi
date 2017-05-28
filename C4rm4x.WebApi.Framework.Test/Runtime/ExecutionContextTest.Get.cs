#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

#endregion

namespace C4rm4x.WebApi.Framework.Test.Runtime
{
    public partial class ExecutionContextTest
    {
        [TestClass]
        public class ExecutionContextGetTest :
            ExecutionContextFixture
        {
            [TestMethod, UnitTest]
            [ExpectedException(typeof(ArgumentException))]
            public void Get_Throws_Exception_When_No_ExecutionContextExtension_Of_Specified_Type_Exists()
            {
                _sut.Get<TestExtension>();
            }

            [TestMethod, UnitTest]
            public void Get_Returns_ExecutionContextExtension_Of_Specified_Type_When_Exists()
            {
                var extension = new TestExtension();

                _sut.Add(extension);

                Assert.AreSame(
                    extension,
                    _sut.Get<TestExtension>());
            }
        }
    }
}
