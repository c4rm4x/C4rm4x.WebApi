#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.Validation;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

#endregion

namespace C4rm4x.WebApi.Framework.RequestHandling.Results
{
    /// <summary>
    /// Returns the HTTP code 400 to indicate the request is invalid
    /// </summary>
    public class BadRequestResult : IHttpActionResult
    {
        /// <summary>
        /// Gets the errors that make the request fail
        /// </summary>
        public List<ValidationError> Errors { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="errors">The validation errors</param>
        public BadRequestResult(List<ValidationError> errors)
        {
            errors.NotNullOrEmpty(nameof(errors));

            Errors = errors;
        }

        /// <summary>
        ///  Creates an System.Net.Http.HttpResponseMessage asynchronously
        /// </summary>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns>The task containing the HTTP response with the 403 code</returns>        
        public Task<HttpResponseMessage> ExecuteAsync(
            CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        private HttpResponseMessage Execute()
        {
            return HttpResponseMessageUtils.Create(
                HttpStatusCode.BadRequest,
                GetBadRequest());
        }

        private BadRequest GetBadRequest()
        {
            return new BadRequest(
                Errors.Select(e => new SerializableValidationError(
                    e.PropertyName, e.ErrorDescription)));
        }
    }

    [DataContract]
    internal class BadRequest
    {
        [DataMember(IsRequired = true)]
        public IEnumerable<SerializableValidationError> ValidationErrors { get; set; }

        public BadRequest()
            : this(new SerializableValidationError[] { })
        {
        }

        public BadRequest(IEnumerable<SerializableValidationError> validationErrors)
        {
            validationErrors.NotNull(nameof(validationErrors));

            ValidationErrors = validationErrors;
        }
    }

    [DataContract]
    internal class SerializableValidationError
    {
        [DataMember(IsRequired = true)]
        public string PropertyName { get; set; }

        [DataMember(IsRequired = true)]
        public string ErrorDescription { get; set; }

        public SerializableValidationError()
        {
        }

        public SerializableValidationError(
            string propertyName,
            string errorDescription)
        {
            propertyName.NotNullOrEmpty(nameof(propertyName));
            errorDescription.NotNullOrEmpty(nameof(errorDescription));

            PropertyName = propertyName;
            ErrorDescription = errorDescription;
        }
    }
}
