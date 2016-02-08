#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

#endregion

namespace C4rm4x.WebApi.Framework.Test.Specification
{

    public partial class ExpressionSpecificationTest
    {
        [TestClass]
        public class ExpressionSpecificationEnforceRuleTest
        {
            [TestMethod, UnitTest]
            public void EnforceRule_Does_Not_Throw_Exception_When_Specification_Is_Fulfilled()
            {
                GetSubjectUnderTest(true)
                    .EnforceRule(ObjectMother.Create<TestEntity>());
            }

            [TestMethod, UnitTest]
            [ExpectedException(typeof(Exception))]
            public void EnforceRule_Throws_Exception_When_Specification_Is_Not_Fulfilled()
            {
                GetSubjectUnderTest(false)
                    .EnforceRule(ObjectMother.Create<TestEntity>());
            }
        }
    }
}
