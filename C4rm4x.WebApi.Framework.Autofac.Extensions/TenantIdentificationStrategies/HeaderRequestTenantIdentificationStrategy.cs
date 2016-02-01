#region Using

using Autofac.Extras.Multitenant;
using C4rm4x.Tools.HttpUtilities;
using C4rm4x.Tools.Utilities;
using System;

#endregion

namespace C4rm4x.WebApi.Framework.Autofac.Extensions.TenantIdentificationStrategies
{
    /// <summary>
    /// Base implementation of ITenantIdentificationStrategy where the tenantId gets identified
    /// based on the presence of a particular header in the current HTTP request
    /// </summary>
    public abstract class HeaderRequestTenantIdentificationStrategy :
        ITenantIdentificationStrategy
    {
        /// <summary>
        /// The header name where the value is used to calculate the tenant identifier
        /// </summary>
        protected string HeaderName { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="headerName">The header name</param>
        public HeaderRequestTenantIdentificationStrategy(string headerName)
        {
            headerName.NotNullOrEmpty(nameof(headerName));

            HeaderName = headerName;
        }

        /// <summary>
        /// Attempts to identify the tenant from the current execution context
        /// </summary>
        /// <param name="tenantId">The current tenant identifier</param>
        /// <returns>True if the tenant could be identified; false, otherwise</returns>
        public bool TryIdentifyTenant(out object tenantId)
        {
            try
            {
                var context = HttpContextFactory.Current;

                context.NotNull("HttpContext.Current");
                context.Request.NotNull("HttpContext.Current.Request");
                context.Request.Headers[HeaderName].NotNull(HeaderName);

                tenantId = GetTenantId(context.Request.Headers[HeaderName]);
            }
            catch (Exception)
            {
                tenantId = null;
            }

            return tenantId != null;
        }

        /// <summary>
        /// Retrieves the current tenant identifier based on header value
        /// </summary>
        /// <param name="headerValue">The value of the header with name headerName</param>
        /// <returns>The tenant identifier</returns>
        protected abstract object GetTenantId(string headerValue);
    }
}
