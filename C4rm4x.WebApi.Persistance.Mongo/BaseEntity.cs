#region Using

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

#endregion

namespace C4rm4x.WebApi.Persistance.Mongo
{
    /// <summary>
    /// Base entity to use MongoDB repositories
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}
