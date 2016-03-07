#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.RequestHandling;
using C4rm4x.WebApi.Framework.RequestHandling.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Web.Http;

#endregion

namespace C4rm4x.WebApi.Framework.Test.RequestHandling
{
    public partial class AbstractApiControllerTest
    {
        [TestClass]
        public class AbstractApiControllerHandleTest
        {
            #region Helper classes

            public class TestApiController : AbstractApiController
            {
                public TestApiController(IHandlerFactory factory)
                    : base(factory)
                {
                }

                public IHttpActionResult TestMethod(TestRequest request)
                {
                    return Handle(request);
                }
            }

            public class TestRequest : ApiRequest
            { }

            public class TestResponse : ApiResponse
            { }

            #endregion

            [TestMethod, UnitTest]
            public void Handle_Uses_IHandlerFactory_To_Retrieve_The_Instance_Of_Handler_Responsible_For_Specific_Request_Type()
            {
                var handlerFactory = Mock.Of<IHandlerFactory>();

                CreateSubjectUnderTest(handlerFactory: handlerFactory)
                    .TestMethod(ObjectMother.Create<TestRequest>());

                Mock.Get(handlerFactory)
                    .Verify(f => f.GetHandler<TestRequest>(), Times.Once());
            }

            [TestMethod, UnitTest]
            public void Handle_Uses_The_Instance_Of_Handler_Responsible_For_Specific_Request_To_Process_The_Request()
            {
                var handler = GetHandler();
                var request = ObjectMother.Create<TestRequest>();

                CreateSubjectUnderTest(handler: handler)
                    .TestMethod(request);

                Mock.Get(handler)
                    .Verify(h => h.Handle(request), Times.Once());
            }

            [TestMethod, UnitTest]
            public void Handle_Returns_The_Same_IHttpActionResult_Returned_By_The_Instance_Of_Handler_Responsible_For_Specific_Request()
            {
                var result = new OkResult<TestResponse>(new TestResponse());

                Assert.AreSame(
                    result,
                    CreateSubjectUnderTest(handler: GetHandler(result))
                        .TestMethod(ObjectMother.Create<TestRequest>()));
            }

            private static TestApiController CreateSubjectUnderTest(
                IHandlerFactory handlerFactory = null,
                IHandler<TestRequest> handler = null)
            {
                return new TestApiController(
                    GetHandlerFactory(handlerFactory, handler));
            }

            private static IHandlerFactory GetHandlerFactory(
                IHandlerFactory handlerFactory, 
                IHandler<TestRequest> handler)
            {
                if (handlerFactory.IsNull())
                    handlerFactory = Mock.Of<IHandlerFactory>();

                if (handler.IsNull())
                    handler = GetHandler();

                Mock.Get(handlerFactory)
                    .Setup(f => f.GetHandler<TestRequest>())
                    .Returns(handler);

                return handlerFactory;
            }

            private static IHandler<TestRequest> GetHandler()
            {
                var handler = Mock.Of<IHandler<TestRequest>>();

                SetupHandler(handler);

                return handler;
            }

            private static IHandler<TestRequest> GetHandler(IHttpActionResult result)
            {
                var handler = GetHandler();

                SetupHandler(handler, result: result);

                return handler;
            }

            private static void SetupHandler(
                IHandler<TestRequest> handler, 
                IHttpActionResult result = null)
            {
                Mock.Get(handler)
                    .Setup(h => h.Handle(It.IsAny<TestRequest>()))
                    .Returns(result ?? new OkResult<TestResponse>(new TestResponse())); 
            }
        }
    }
}
