namespace C4rm4x.WebApi.ExceptionShielding.Configuration
{
    /// <summary>
    /// Fluent api for PostHandlingAction
    /// </summary>
    public static class Then
    {
        /// <summary>
        /// Does nothing when exception arises
        /// </summary>
        /// <returns>PostHandlingAction.None</returns>
        public static PostHandlingAction DoNothing()
        {
            return PostHandlingAction.None;
        }

        /// <summary>
        /// Rethrows original exception when this one arises
        /// </summary>
        /// <returns>PostHandlingAction.ShouldRethrow</returns>
        public static PostHandlingAction Rethrow()
        {
            return PostHandlingAction.ShouldRethrow;
        }

        /// <summary>
        /// Throws new exception when original one arises
        /// </summary>
        /// <returns>PostHandlingAction.ThrowNewException</returns>
        public static PostHandlingAction ThrowNewException()
        {
            return PostHandlingAction.ThrowNewException;
        }
    }
}
