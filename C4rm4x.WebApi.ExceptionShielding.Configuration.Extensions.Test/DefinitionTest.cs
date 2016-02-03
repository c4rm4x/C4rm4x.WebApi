#region Using

using Moq;
using System;

#endregion

namespace C4rm4x.WebApi.ExceptionShielding.Configuration.Test
{
    public partial class DefinitionTest
    {
        private static Entry GetEntry()
        {
            var entry = Entry.For<Exception>();

            entry
                .Then(PostHandlingAction.None)
                .WithExceptionHandler(GetExceptionHandlerData());

            return entry;
        }

        private static IExceptionHandlerData GetExceptionHandlerData()
        {
            return Mock.Of<IExceptionHandlerData>();
        }
    }
}
