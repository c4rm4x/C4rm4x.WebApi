#region Using

using System.Runtime.Serialization;

#endregion

namespace C4rm4x.WebApi.Messaging.AzureQueue.Test.Infrastructure
{
    [DataContract]
    public class TestMessage
    {
        [DataMember(IsRequired = true)]
        public string Value { get; set; }

        private TestMessage()
        {

        }

        public TestMessage(string value)
        {
            Value = value;
        }
    }
}
