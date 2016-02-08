#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

#endregion

namespace C4rm4x.WebApi.Framework.Test.Specification
{

    public partial class OrSpecificationTest
    {
        [TestClass]
        public class OrSpecificationEnforceRuleTest
        {
            [TestMethod, UnitTest]
            public void EnforceRule_Does_Not_Throw_Exception_When_At_Least_LeftSpecification_Is_Fulfilled()
            {
                GetSubjectUnderTest(true, It.IsAny<bool>())
                    .EnforceRule(ObjectMother.Create<TestEntity>());
            }

            [TestMethod, UnitTest]
            public void EnforceRule_Does_Not_Throw_Exception_When_At_Least_RightSpecification_Is_Fulfilled()
            {
                GetSubjectUnderTest(It.IsAny<bool>(), true)
                    .EnforceRule(ObjectMother.Create<TestEntity>());
            }

            [TestMethod, UnitTest]
            public void EnforceRule_Does_Not_Throw_Exception_When_Both_LeftSpecification_And_RightSpecification_Are_Fulfilled()
            {
                GetSubjectUnderTest(true, true)
                    .EnforceRule(ObjectMother.Create<TestEntity>());
            }

            [TestMethod, UnitTest]
            [ExpectedException(typeof(Exception))]
            public void EnforceRule_Throws_Exception_When_Both_LeftSpecification_And_RightSpecification_Are_Not_Fulfilled()
            {
                GetSubjectUnderTest(false, false)
                    .EnforceRule(ObjectMother.Create<TestEntity>());
            }
        }
    }
}
