#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace C4rm4x.WebApi.Validation.Test
{
    public partial class AbstractValidatorTest
    {
        [TestClass]
        public class AbstractValidatorCanValidateInstancesOfTest
        {
            [TestMethod, UnitTest]
            public void CanValidateInstancesOf_Returns_True_When_Type_Is_TestClass()
            {
                Assert.IsTrue(
                    GetValidator().CanValidateInstancesOf(typeof(TestClass)));
            }

            [TestMethod, UnitTest]
            public void CanValidateInstancesOf_Returns_False_When_Type_Is_Not_TestClass()
            {
                Assert.IsFalse(
                    GetValidator().CanValidateInstancesOf(typeof(object)));
            }
        }
    }
}
