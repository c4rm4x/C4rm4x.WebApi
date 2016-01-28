#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Framework.RequestHandling;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

#endregion

namespace C4rm4x.WebApi.Framework.Test.RequestHandling
{
    public partial class HandlerTest
    {
        [TestClass]
        public class HandlerHandleTest
        {
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

            private class TestRequestHandler : Handler<TestRequest, TestResponse>
            {
                public object Result { get; private set; }

                public TestRequestHandler(
                    IHandlerShell shell,
                    object result)
                    : base(shell)
                {
                    Result = result;
                }

                protected override TestResponse Process(TestRequest request)
                {
                    return new TestResponse(Result);
                }
            }

            #endregion

            [TestMethod, UnitTest]
            public void Handle_Uses_IHandleShell_To_Process_The_Request()
            {
                var shell = Mock.Of<IHandlerShell>();
                var request = new TestRequest();

                CreateSubjectUnderTest(shell: shell)
                    .Handle(request);

                Mock.Get(shell)
                    .Verify(s =>
                        s.Process(request, It.IsAny<Func<TestRequest, TestResponse>>()),
                        Times.Once());
            }

            private static IHandler<TestRequest> CreateSubjectUnderTest(
                object result = null,
                IHandlerShell shell = null)
            {
                return new TestRequestHandler(
                    shell ?? Mock.Of<IHandlerShell>(),
                    result ?? ObjectMother.Create<string>());
            }
        }
    }
}
