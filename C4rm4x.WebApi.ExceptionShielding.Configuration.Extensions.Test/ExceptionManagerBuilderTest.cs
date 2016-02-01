#region Using

using Moq;
using System;

#endregion

namespace C4rm4x.WebApi.ExceptionShielding.Configuration.Extensions.Test
{
    public partial class ExceptionManagerBuilderTest
    {
        private const string PolicyName = "PolicyName";

        private static Definition GetDefinition()
        {
            return Definition
                .WithName(PolicyName)
                .WithExceptionPolicyEntry(GetEntry());
        }

        private static Entry GetEntry()
        {
            var entry = Entry
                .For<Exception>();

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
