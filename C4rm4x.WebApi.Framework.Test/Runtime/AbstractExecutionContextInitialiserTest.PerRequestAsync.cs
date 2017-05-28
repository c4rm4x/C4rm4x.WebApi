#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Framework.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Framework.Test.Runtime
{
    public partial class AbstractExecutionContextInitialiserTest
    {
        [TestClass]
        public class AbstractExecutionContextInitialiserPerRequestAsyncTest
        {
            #region Helper classes

            public class TestRequest : ApiRequest { }

            private class TestExecutionContextInitialiser :
                AbstractExecutionContextInitialiser
            {
                private readonly IDictionary<Type, IEnumerable>
                    _initialisers;

                public TestExecutionContextInitialiser(
                    IExecutionContext executionContext)
                    : base(executionContext)
                {
                    _initialisers = new Dictionary<Type, IEnumerable>();
                }

                public void SetExecutionContextInitialiers<TRequest>(
                    params IExecutionContextExtensionInitialiser<TRequest>[] initialisers)
                    where TRequest : ApiRequest
                {
                    _initialisers.Add(typeof(TRequest), initialisers);
                }

                protected override IEnumerable<IExecutionContextExtensionInitialiser<TRequest>>
                    GetInitialisersPerRequest<TRequest>()
                {
                    return (_initialisers.ContainsKey(typeof(TRequest)))
                        ? _initialisers[typeof(TRequest)].OfType<IExecutionContextExtensionInitialiser<TRequest>>()
                        : new IExecutionContextExtensionInitialiser<TRequest>[] { };
                }
            }

            #endregion

            [TestMethod, UnitTest]
            public async Task PerRequestAsync_Uses_All_ExecutionContextExtensionInitialisers_To_Initialise_The_Current_ExecutionContext()
            {
                var request = new TestRequest();
                var executionContext = Mock.Of<IExecutionContext>();
                var executionContextExtensionInitialisers =
                    GetExecutionContextExtensionInitialisers<TestRequest>(GetRand(10))
                    .ToArray(); // To get the final list

                await CreateSubjectUnderTest(
                    executionContext,
                    executionContextExtensionInitialisers)
                    .PerRequestAsync(request);

                foreach (var executionContextExtensionInitialiser in
                    executionContextExtensionInitialisers)
                    Mock.Get(executionContextExtensionInitialiser)
                        .Verify(e => e.AppendAsync(request), Times.Once());
            }

            private static TestExecutionContextInitialiser
                CreateSubjectUnderTest(IExecutionContext executionContext)
            {
                return new TestExecutionContextInitialiser(executionContext);
            }

            private static TestExecutionContextInitialiser
                CreateSubjectUnderTest<TRequest>(
                IExecutionContext executionContext,
                params IExecutionContextExtensionInitialiser<TRequest>[] initialisers)
                where TRequest : ApiRequest
            {
                var executionContextInitialiser =
                    CreateSubjectUnderTest(executionContext);

                executionContextInitialiser
                    .SetExecutionContextInitialiers(initialisers);

                return executionContextInitialiser;
            }

            private static int GetRand(int max)
            {
                return new Random().Next(1, max);
            }

            private static IEnumerable<IExecutionContextExtensionInitialiser<TRequest>>
                GetExecutionContextExtensionInitialisers<TRequest>(
                int numberOfExecutionContextExtensionInitialisers)
                where TRequest : ApiRequest
            {
                for (int i = 0; i < numberOfExecutionContextExtensionInitialisers; i++)
                    yield return Mock.Of<IExecutionContextExtensionInitialiser<TRequest>>();
            }
        }
    }
}
