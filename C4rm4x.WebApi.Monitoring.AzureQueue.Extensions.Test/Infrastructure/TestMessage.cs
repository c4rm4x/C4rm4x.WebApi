#region Using

using C4rm4x.Tools.Utilities;
using System.Runtime.Serialization;

#endregion

namespace C4rm4x.WebApi.Monitoring.AzureQueue.Test
{
    [DataContract]
    public class TestMessage
    {
        [DataMember(IsRequired = true)]
        public string Property { get; private set; }

        public TestMessage(string property)
        {
            property.NotNullOrEmpty(nameof(property));

            Property = property;
        }
    }
}
