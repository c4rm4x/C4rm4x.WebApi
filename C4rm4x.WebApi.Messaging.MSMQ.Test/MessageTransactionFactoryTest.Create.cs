#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Messaging;

#endregion

namespace C4rm4x.WebApi.Messaging.MSMQ.Test
{
    public partial class MessageTransactionFactoryTest
    {
        [TestClass]
        public class MessageTransactionFactoryCreateTest
            : AutoMockFixture<MessageTransactionFactory>
        {
            [TestMethod, UnitTest]
            public void Create_Creates_A_New_Instance_Of_MessageQueueTransaction()
            {
                var result = _sut.Create();

                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(MessageQueueTransaction));
            }

            [TestMethod, UnitTest]
            public void Create_Creates_A_New_Instance_Of_MessageQueueTransaction_With_Status_Pending()
            {
                var result = _sut.Create() as MessageQueueTransaction;

                Assert.AreEqual(
                    MessageQueueTransactionStatus.Pending,
                    result.Status);
            }

            [TestMethod, UnitTest]
            public void Create_Creates_A_New_Instance_Of_MessageQueueTransaction_Which_Changes_Its_Status_To_Commited_When_It_Gets_Disposed()
            {
                var result = _sut.Create() as MessageQueueTransaction;

                result.Dispose();

                Assert.AreEqual(
                    MessageQueueTransactionStatus.Committed,
                    result.Status);
            }
        }
    }
}
