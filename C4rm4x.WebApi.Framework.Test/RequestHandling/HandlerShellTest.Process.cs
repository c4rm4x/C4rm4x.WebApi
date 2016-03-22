#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Framework.ExceptionShielding;
using C4rm4x.WebApi.Framework.RequestHandling;
using C4rm4x.WebApi.Framework.Runtime;
using C4rm4x.WebApi.Framework.Test.Builders;
using C4rm4x.WebApi.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Web.Http;

#endregion

namespace C4rm4x.WebApi.Framework.Test.RequestHandling
{
    public partial class HandlerShellTest
    {
        [TestClass]
        public class HandlerShellProcessTest :
            AutoMockFixture<HandlerShell>
        {
            private const string GenericErrorMessage = "Error";
            private const string PolicyName = "default";

            #region Helper classes

            private class TestRequest : ApiRequest { }

            private class TestResponse : ApiResponse
            {
                public object Result { get; private set; }

                public TestResponse(object result)
                {
                    Result = result;
                }
            }

            #endregion

            [TestInitialize]
            public override void Setup()
            {
                base.Setup();

                Returns<IValidatorFactory, IValidator>(
                    f => f.GetValidator(typeof(TestRequest)),
                    Mock.Of<IValidator>());

                ExceptionPolicy.SetExceptionManager(GetExceptionManager());

                _sut.PolicyName = PolicyName;
            }

            private static IExceptionManager GetExceptionManager()
            {
                var exceptionManager = Mock.Of<IExceptionManager>();

                Mock.Get(exceptionManager)
                    .Setup(m => m.HandleException(
                        It.IsAny<Exception>(), PolicyName))
                    .Throws(new Exception(GenericErrorMessage));

                Mock.Get(exceptionManager)
                    .Setup(m => m.HandleException(
                        It.IsAny<ValidationException>(), PolicyName))
                    .Returns(true);

                return exceptionManager;
            }

            [TestMethod, UnitTest]
            public void Process_Returns_Ok_When_Everything_Goes_Well()
            {
                var result = _sut.Process<TestRequest, TestResponse>(new TestRequest(), Process);

                Assert.AreEqual(HttpStatusCode.OK, GetStatusCode(result));
            }

            [TestMethod, UnitTest]
            public void Process_Returns_BadRequest_When_A_ValidationError_Is_Thrown()
            {
                var result = _sut.Process<TestRequest, TestResponse>(new TestRequest(), request =>
                {
                    throw new ValidationException(GetValidationErrors());
                });

                Assert.AreEqual(HttpStatusCode.BadRequest, GetStatusCode(result));
            }

            [TestMethod, UnitTest]
            public void Process_Returns_InternalServerError_When_An_Exception_Different_To_ValidationException_Is_Thrown()
            {
                var result = _sut.Process<TestRequest, TestResponse>(new TestRequest(), request =>
                {
                    throw new Exception(GenericErrorMessage);
                });

                Assert.AreEqual(HttpStatusCode.InternalServerError, GetStatusCode(result));
            }

            [TestMethod, UnitTest]
            public void Process_Uses_IValidatorFactory_To_Retrieve_The_Most_Specific_Request_Validator()
            {
                var request = new TestRequest();

                _sut.Process<TestRequest, TestResponse>(request, Process);

                Verify<IValidatorFactory>(e => e.GetValidator(request.GetType()), Times.Once());
            }

            [TestMethod, UnitTest]
            public void Process_Uses_IExecutionContextInitialiser_To_Initialise_Current_ExecutionContext_Per_Request()
            {
                var request = new TestRequest();

                _sut.Process<TestRequest, TestResponse>(request, Process);

                Verify<IExecutionContextInitialiser>(e => e.PerRequest(request), Times.Once());
            }

            private static HttpStatusCode GetStatusCode(IHttpActionResult actionResult)
            {
                return actionResult
                    .ExecuteAsync(It.IsAny<CancellationToken>())
                    .Result
                    .StatusCode;
            }

            private static TestResponse Process(TestRequest request)
            {
                return new TestResponse(ObjectMother.Create<string>());
            }

            private static IEnumerable<ValidationError> GetValidationErrors()
            {
                var numberOfValidationErrors = GetRand(10);

                for (var i = 0; i < numberOfValidationErrors; i++)
                    yield return new ValidationErrorBuilder().Build();
            }

            private static int GetRand(int max)
            {
                return new Random().Next(1, max);
            }
        }
    }
}
