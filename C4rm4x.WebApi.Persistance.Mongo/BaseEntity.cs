#region Using

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

#endregion

namespace C4rm4x.WebApi.Persistance.Mongo
{
    public abstract class BaseEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}
