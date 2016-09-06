#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Transactions;

#endregion

namespace C4rm4x.WebApi.Messaging.ServiceBus.Test
{
    public partial class MessageTransactionFactoryTest
    {
        [TestClass]
        public class MessageTransactionFactoryCreateTest :
            AutoMockFixture<MessageTransactionFactory>
        {
            [TestMethod, UnitTest]
            public void Create_Creates_A_New_Instance_Of_MessageTransaction()
            {
                var result = _sut.Create();

                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(MessageTransaction));
            }

            [TestMethod, UnitTest]
            public void Create_Creates_A_New_Instance_Of_MessageTransaction_With_Internal_TransactionScope()
            {
                var result = _sut.Create() as MessageTransaction;

                Assert.IsNotNull(result.Scope);
            }

            [TestMethod, UnitTest]
            public void Create_Creates_A_New_Instance_Of_MessageTransaction_Which_Completes_Internal_TransactionScope_When_It_Gets_Disposed()
            {
                var isCompleted = false;

                var result = _sut.Create() as MessageTransaction;

                Transaction.Current.TransactionCompleted += (sender, e) =>
                {
                    isCompleted = true;
                };

                result.Dispose();

                Assert.IsTrue(isCompleted);
            }
        }
    }
}
