#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Framework.RequestHandling.Results;
using C4rm4x.WebApi.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

#endregion

namespace C4rm4x.WebApi.Framework.Test.RequestHandling.Results
{
    public class BadRequestResultTest
    {
        [TestClass]
        public class BadRequestResultExecuteAsyncTest
        {
            [TestMethod, UnitTest]
            public void ExecuteAsync_Returns_BadRequest_Response()
            {
                Assert.AreEqual(
                    HttpStatusCode.BadRequest,
                    ExecuteAsync().Result.StatusCode);
            }

            [TestMethod, UnitTest]
            public void ExecuteAsync_Sets_ContentType_Header_MediaType_As_ApplicationJson()
            {
                Assert.AreEqual(
                    "application/json",
                    ExecuteAsync().Result.Content.Headers.ContentType.MediaType);
            }

            [TestMethod, UnitTest]
            public void ExecuteAsync_Returns_Content_As_IEnumerable()
            {
                Assert.IsInstanceOfType(
                    ExecuteAsync().Result.Content,
                    typeof(ObjectContent<IEnumerable>));
            }
            
            [TestMethod, UnitTest]
            public void ExecuteAsync_Maps_Each_ValidationErrors_With_Its_Equivalent_In_Content()
            {
                var PropertyName = ObjectMother.Create(10);
                var ErrorDescription = ObjectMother.Create(20);
                
                var content = ExecuteAsync(
                    new ValidationError(
                        PropertyName, ObjectMother.Create(10), ErrorDescription)).Result.Content;
                                        
                var errors = (content as ObjectContent).Value as IEnumerable;
                Assert.IsTrue(errors.Any());
                
                dynamic error = errors.FirstOrDefault();
                Assert.IsNotNull(error);         
                Assert.AreEqual(PropertyName, error.PropertyName);
                Assert.AreEqual(ErrorDescription, error.ErrorDescription);                                                               
            }

            private static BadRequestResult CreateSubjectUnderTest(
                params ValidationError[] validationErrors)
            {
                return new BadRequestResult(
                    new ValidationException(validationErrors));
            }

            private static Task<HttpResponseMessage> ExecuteAsync(
                params ValidationError[] validationErrors)
            {
                return CreateSubjectUnderTest(validationErrors)
                    .ExecuteAsync(It.IsAny<CancellationToken>());
            }
        }
    }
}
