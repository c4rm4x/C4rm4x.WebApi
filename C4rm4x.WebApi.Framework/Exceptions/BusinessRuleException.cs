namespace C4rm4x.WebApi.Framework
{
    /// <summary>
    /// Base exception to indicate when a business rule has been violated
    /// </summary>
    public abstract class BusinessRuleException : ApiException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="code">The business rule code</param>
        /// <param name="message">The exception message</param>
        public BusinessRuleException(string code, string message)
            : base(code, message)
        { }
    }
}