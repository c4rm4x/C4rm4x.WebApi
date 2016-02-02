#region Using

using Autofac.Extras.Multitenant;
using C4rm4x.Tools.HttpUtilities;
using C4rm4x.Tools.Utilities;
using System;
using System.Text.RegularExpressions;

#endregion

namespace C4rm4x.WebApi.Framework.Autofac.TenantIdentificationStrategies
{
    /// <summary>
    /// Implementation of the ITenantIdentificationStrategy based on the 
    /// current HTTP request URL
    /// </summary>
    public abstract class UrlRequestTenantIdentificationStrategy
         : ITenantIdentificationStrategy
    {
        /// <summary>
        /// Url pattern used to calculate the tenant identifier
        /// </summary>
        protected string UrlPattern { get; private set; }

        /// <summary>
        /// Regex options to check whether or not the current HTTP request url matches the URL pattern
        /// </summary>
        protected RegexOptions RegexOptions { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="urlPattern">The URL pattern</param>
        /// <param name="options">Regex options</param>
        public UrlRequestTenantIdentificationStrategy(
            string urlPattern,
            RegexOptions options = RegexOptions.None)
        {
            urlPattern.NotNullOrEmpty(nameof(urlPattern));

            UrlPattern = urlPattern;
            RegexOptions = options;
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
                context.Request.Url.NotNull("HttpContext.Current.Request.Url");
                context.Request.Url.AbsolutePath
                    .NotNullOrEmpty("HttpContext.Current.Request.Url.AbsolutePaht");

                tenantId = GetTenantId(
                    Regex.Match(context.Request.Url.AbsolutePath, UrlPattern, RegexOptions));
            }
            catch (Exception)
            {
                tenantId = null;
            }

            return tenantId != null;
        }

        /// <summary>
        /// Retrieves the current tenant identifier based on url match
        /// </summary>
        /// <param name="urlMatch">The match after checking whether or not the current HTTP request url matches the pattern</param>
        /// <returns>The tenant identifier</returns>
        protected abstract object GetTenantId(Match urlMatch);
    }
}
