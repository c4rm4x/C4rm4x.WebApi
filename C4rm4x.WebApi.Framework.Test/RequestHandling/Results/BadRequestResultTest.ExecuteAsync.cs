#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Framework.RequestHandling.Results;
using C4rm4x.WebApi.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

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
                    ExecuteAsync(GetValidationErrors().ToArray()).Result.StatusCode);
            }

            [TestMethod, UnitTest]
            public void ExecuteAsync_Sets_ContentType_Header_MediaType_As_ApplicationJson()
            {
                Assert.AreEqual(
                    "application/json",
                    ExecuteAsync(GetValidationErrors().ToArray()).Result.Content.Headers.ContentType.MediaType);
            }

            [TestMethod, UnitTest]
            public void ExecuteAsync_Returns_Content_As_BadRequest()
            {
                Assert.IsInstanceOfType(
                    ExecuteAsync(GetValidationErrors().ToArray()).Result.Content,
                    typeof(ObjectContent<BadRequest>));
            }
            
            [TestMethod, UnitTest]
            public void ExecuteAsync_Maps_Each_ValidationErrors_With_Its_Equivalent_SerializableValidationError_WIthin_Content()
            {
                var validationErrors = GetValidationErrors().ToArray();

                var content = ExecuteAsync(validationErrors).Result.Content;
                var objectContent = content as ObjectContent<BadRequest>;
                var badRequest = objectContent.Value as BadRequest;

                Assert.IsTrue(badRequest.ValidationErrors.Any());
                Assert.AreEqual(validationErrors.Count(), badRequest.ValidationErrors.Count());

                foreach (var validationError in validationErrors)
                    Assert.IsNotNull(badRequest.ValidationErrors.FirstOrDefault(v =>
                        v.PropertyName == validationError.PropertyName &&
                        v.ErrorDescription == validationError.ErrorDescription));                                                                             
            }

            private static BadRequestResult CreateSubjectUnderTest(
                params ValidationError[] validationErrors)
            {
                return new BadRequestResult(validationErrors.ToList());
            }

            private static Task<HttpResponseMessage> ExecuteAsync(
                params ValidationError[] validationErrors)
            {
                return CreateSubjectUnderTest(validationErrors)
                    .ExecuteAsync(It.IsAny<CancellationToken>());
            }

            private static IEnumerable<ValidationError> GetValidationErrors()
            {
                var numberOfValidationErrors = GetRand(10);

                for (var i = 0; i < numberOfValidationErrors; i++)
                    yield return new ValidationError(
                        ObjectMother.Create(10), null, ObjectMother.Create(100));
            }

            private static int GetRand(int max)
            {
                return new Random().Next(1, max);
            }
        }
    }
}
