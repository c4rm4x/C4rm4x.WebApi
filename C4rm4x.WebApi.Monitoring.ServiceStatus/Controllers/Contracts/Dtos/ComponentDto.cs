#region Using

using System.Runtime.Serialization;

#endregion

namespace C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Contracts.Dtos
{
    /// <summary>
    /// Description of a component in your system
    /// </summary>
    [DataContract]
    public class ComponentDto
    {
        /// <summary>
        /// Parameterless constructor for serialization/deserialization
        /// </summary>
        public ComponentDto()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="identifier">The component's identifier</param>
        /// <param name="name">The component's name</param>
        public ComponentDto(
            object identifier,
            string name)
        {
            Identifier = identifier;
            Name = name;
        }

        /// <summary>
        /// The component's identifier
        /// </summary>
        [DataMember(IsRequired = true)]
        public object Identifier { get; set; }

        /// <summary>
        /// The component's name
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Name { get; set; }
    }
}
