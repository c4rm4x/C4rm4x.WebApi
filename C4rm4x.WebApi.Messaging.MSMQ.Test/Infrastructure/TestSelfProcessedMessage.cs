#region Using

using System;

#endregion

namespace C4rm4x.WebApi.Messaging.MSMQ.Test.Infrastructure
{
    [Serializable]
    public class TestSelfProcessedMessage
    {
        public string Message { get; set; }

        public TestSelfProcessedMessage()
            : this("")
        { }

        public TestSelfProcessedMessage(
            string message)
        {
            Message = message;
        }
    }
}
