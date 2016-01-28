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
        public class ExecutionContextGetExtensionTest :
            ExecutionContextFixture
        {
            [TestMethod, UnitTest]
            [ExpectedException(typeof(ArgumentException))]
            public void GetExtension_Throws_Exception_When_No_ExecutionContextExtension_Of_Specified_Type_Exists()
            {
                _sut.GetExtension<TestExecutionContextExtension>();
            }

            [TestMethod, UnitTest]
            public void GetExtension_Returns_ExecutionContextExtension_Of_Specified_Type_When_Exists()
            {
                var Extension = new TestExecutionContextExtension();

                _sut.AddExtension(Extension);

                Assert.AreSame(
                    Extension,
                    _sut.GetExtension<TestExecutionContextExtension>());
            }
        }
    }
}
