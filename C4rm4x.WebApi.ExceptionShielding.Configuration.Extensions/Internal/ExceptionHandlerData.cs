#region Using

using C4rm4x.Tools.Utilities;
using System;

#endregion

namespace C4rm4x.WebApi.ExceptionShielding.Configuration
{
    internal class ExceptionHandlerData<THandler> : IExceptionHandlerData
        where THandler : IExceptionHandler
    {
        private readonly Type _exceptionType;
        private readonly string _exceptionMessage;

        public ExceptionHandlerData(
            Type exceptionType,
            string exceptionMessage)
        {
            exceptionType.NotNull(nameof(exceptionType));
            exceptionType.Is<Exception>();
            exceptionMessage.NotNullOrEmpty(nameof(exceptionMessage));

            _exceptionType = exceptionType;
            _exceptionMessage = exceptionMessage;
        }

        public IExceptionHandler GetExceptionHandler()
        {
            return (IExceptionHandler)Activator.CreateInstance(
                typeof(THandler),
                new object[] { _exceptionMessage, _exceptionType });
        }
    }
}
