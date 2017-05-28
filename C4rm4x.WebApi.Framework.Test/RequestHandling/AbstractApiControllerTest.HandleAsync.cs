#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.RequestHandling;
using C4rm4x.WebApi.Framework.RequestHandling.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using System.Web.Http;

#endregion

namespace C4rm4x.WebApi.Framework.Test.RequestHandling
{
    public partial class AbstractApiControllerTest
    {
        [TestClass]
        public class AbstractApiControllerHandleAsyncTest
        {
            #region Helper classes

            public class TestApiController : AbstractApiController
            {
                public TestApiController(IHandlerShell shell)
                    : base(shell)
                {
                }

                public async Task<IHttpActionResult> TestMethod(TestRequest request)
                {
                    return await HandleAsync(request);
                }
            }

            public class TestRequest : ApiRequest
            { }

            #endregion

            [TestMethod, UnitTest]
            public async Task HandleAsync_Uses_IHandlerShell_To_Handle_Request()
            {
                var shell = Mock.Of<IHandlerShell>();
                var request = ObjectMother.Create<TestRequest>();

                await CreateSubjectUnderTest(shell: shell)
                    .TestMethod(request);

                Mock.Get(shell)
                    .Verify(s => s.HandleAsync<TestRequest>(request), 
                    Times.Once());
            }            

            [TestMethod, UnitTest]
            public void HandleAsync_Returns_The_Same_IHttpActionResult_Returned_By_The_Instance_Of_Handler_Responsible_For_Specific_Request()
            {
                var result = new NoContentResult();

                Assert.AreSame(
                    result,
                    CreateSubjectUnderTest(result: result)
                        .TestMethod(ObjectMother.Create<TestRequest>()).Result);
            }

            private static TestApiController CreateSubjectUnderTest(
                IHandlerShell shell = null,
                IHttpActionResult result = null)
            {
                return new TestApiController(
                    GetHandlerShell(shell, result));
            }

            private static IHandlerShell GetHandlerShell(
                IHandlerShell shell,
                IHttpActionResult result)
            {
                if (shell.IsNull())
                    shell = Mock.Of<IHandlerShell>();

                Mock.Get(shell)
                    .Setup(s => s.HandleAsync(It.IsAny<TestRequest>()))
                    .Returns(Task.FromResult(result ?? new NotFoundResult()));

                return shell;
            }
        }
    }
}
