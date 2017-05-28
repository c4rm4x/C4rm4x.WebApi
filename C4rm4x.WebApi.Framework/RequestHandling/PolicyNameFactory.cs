namespace C4rm4x.WebApi.Framework.RequestHandling
{
    #region Interface

    /// <summary>
    /// Service responsible to retrieve policy name
    /// </summary>
    public interface IPolicyNameFactory
    {
        /// <summary>
        /// Gets the name
        /// </summary>
        /// <returns>The policy name</returns>
        string Get();
    }

    #endregion

    /// <summary>
    /// Base class that implements interface IPolicyNameFactory returning "default"
    /// </summary>
    public class PolicyNameFactory : IPolicyNameFactory
    {
        /// <summary>
        /// Gets the name
        /// </summary>
        /// <returns>The policy name</returns>
        public string Get()
        {
            return "default";
        }
    }
}
