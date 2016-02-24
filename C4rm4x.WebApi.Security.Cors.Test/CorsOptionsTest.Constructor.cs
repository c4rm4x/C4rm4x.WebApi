#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.Tools.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

#endregion

namespace C4rm4x.WebApi.Security.Cors.Test
{
    public partial class CorsOptionsTest
    {
        [TestClass]
        public class CorsOptionsConstructorTest
        {
            [TestMethod, UnitTest]
            [ExpectedException(typeof(ArgumentException))]
            public void Constructor_Throws_AN_Exception_When_AllowedOriginsSeparatedByComma_Is_Null()
            {
                new CorsOptions(allowedOriginsSeparatedByComma: null);
            }

            [TestMethod, UnitTest]
            [ExpectedException(typeof(ArgumentException))]
            public void Constructor_Throws_AN_Exception_When_AllowedOriginsSeparatedByComma_Is_Empty_String()
            {
                new CorsOptions(allowedOriginsSeparatedByComma: string.Empty);
            }

            [TestMethod, UnitTest]
            [ExpectedException(typeof(ArgumentException))]
            public void Constructor_Throws_AN_Exception_When_AllowedOriginsSeparatedByComma_Does_Not_Contain_Any_Origin()
            {
                new CorsOptions(allowedOriginsSeparatedByComma: ",");
            }

            [TestMethod, UnitTest]
            public void Constructor_Creates_A_New_Instance_With_AllowAnyOrigin_As_True_When_AllowedOriginsSeparatedByComma_Is_Star()
            {
                Assert.IsTrue(
                    new CorsOptions(allowedOriginsSeparatedByComma: CorsOptions.AnyOrigin)
                    .AllowAnyOrigin);
            }

            [TestMethod, UnitTest]
            public void Constructor_Creates_A_New_Instance_With_AllowedOrigins_As_Empty_Collection_When_AllowedOriginsSeparatedByComma_Is_Star()
            {
                Assert.IsTrue(
                    new CorsOptions(allowedOriginsSeparatedByComma: CorsOptions.AnyOrigin)
                    .AllowedOrigins
                    .IsNullOrEmpty());
            }

            [TestMethod, UnitTest]
            public void Constructor_Creates_A_New_Instance_With_AllowAnyOrigin_As_False_When_AllowedOriginsSeparatedByComma_Is_An_Specific_Origin()
            {
                Assert.IsFalse(
                    new CorsOptions(allowedOriginsSeparatedByComma: ObjectMother.Create<string>())
                    .AllowAnyOrigin);
            }

            [TestMethod, UnitTest]
            public void Constructor_Creates_A_New_Instance_With_AllowedOrigins_As_Not_Empty_Collection_When_AllowedOriginsSeparatedByComma_Is_An_Specific_Origin()
            {
                Assert.IsTrue(
                    new CorsOptions(allowedOriginsSeparatedByComma: ObjectMother.Create<string>())
                    .AllowedOrigins
                    .Any());
            }

            [TestMethod, UnitTest]
            public void Constructor_Creates_A_New_Instance_With_AllowAnyHeader_As_True_When_AllowedHeadersSeparatedByComma_Is_All()
            {
                Assert.IsTrue(
                    new CorsOptions(allowedHeadersSeparatedByComma: CorsOptions.AnyHeader)
                    .AllowAnyHeader);
            }

            [TestMethod, UnitTest]
            public void Constructor_Creates_A_New_Instance_With_AllowedHeaders_As_Empty_Collection_When_AllowedHeadersSeparatedByComma_Is_All()
            {
                Assert.IsTrue(
                    new CorsOptions(allowedHeadersSeparatedByComma: CorsOptions.AnyHeader)
                    .AllowedHeaders
                    .IsNullOrEmpty());
            }

            [TestMethod, UnitTest]
            public void Constructor_Creates_A_New_Instance_With_AllowAnyHeader_As_False_When_AllowedHeadersSeparatedByComma_Is_An_Specific_Header()
            {
                Assert.IsFalse(
                    new CorsOptions(allowedHeadersSeparatedByComma: ObjectMother.Create<string>())
                    .AllowAnyHeader);
            }

            [TestMethod, UnitTest]
            public void Constructor_Creates_A_New_Instance_With_AllowedHeaders_As_Not_Empty_Collection_When_AllowedHeadersSeparatedByComma_Is_An_Specific_Header()
            {
                Assert.IsTrue(
                    new CorsOptions(allowedHeadersSeparatedByComma: ObjectMother.Create<string>())
                    .AllowedHeaders
                    .Any());
            }

            [TestMethod, UnitTest]
            public void Constructor_Creates_A_New_Instance_With_AllowAnyMethod_As_True_When_AllowedMethodsSeparatedByComma_Is_All()
            {
                Assert.IsTrue(
                    new CorsOptions(allowedMethodsSeparatedByComma: CorsOptions.AnyMethod)
                    .AllowAnyMethod);
            }

            [TestMethod, UnitTest]
            public void Constructor_Creates_A_New_Instance_With_AllowedMethods_As_Empty_Collection_When_AllowedMethodsSeparatedByComma_Is_All()
            {
                Assert.IsTrue(
                    new CorsOptions(allowedMethodsSeparatedByComma: CorsOptions.AnyMethod)
                    .AllowedMethods
                    .IsNullOrEmpty());
            }

            [TestMethod, UnitTest]
            public void Constructor_Creates_A_New_Instance_With_AllowAnyMethod_As_False_When_AllowedMethodsSeparatedByComma_Is_An_Specific_Method()
            {
                Assert.IsFalse(
                    new CorsOptions(allowedMethodsSeparatedByComma: ObjectMother.Create<string>())
                    .AllowAnyMethod);
            }

            [TestMethod, UnitTest]
            public void Constructor_Creates_A_New_Instance_With_AllowedMethods_As_Not_Empty_Collection_When_AllowedMethodsSeparatedByComma_Is_An_Specific_Method()
            {
                Assert.IsTrue(
                    new CorsOptions(allowedMethodsSeparatedByComma: ObjectMother.Create<string>())
                    .AllowedMethods
                    .Any());
            }
        }
    }
}
