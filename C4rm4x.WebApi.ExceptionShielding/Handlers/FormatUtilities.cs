#region Using

using C4rm4x.Tools.Utilities;
using System;

#endregion

namespace C4rm4x.WebApi.ExceptionShielding.Handlers
{
    internal static class FormatUtilities
    {
        private const string HandlingInstanceToken = "{handlingInstanceID}";

        /// <summary>
        /// Replaces {handlingInstanceID} token within message (if any) for the specified Guid value
        /// </summary>
        /// <param name="message">Format which might include {handlingInstanceID} token within</param>
        /// <param name="handlingInstanceId">A Guid value</param>
        /// <returns>The formatted message</returns>
        public static string FormatException(
            string message,
            Guid handlingInstanceId)
        {
            message.NotNullOrEmpty(nameof(message));

            return message.Replace(
                HandlingInstanceToken,
                handlingInstanceId.ToString());
        }
    }
}
