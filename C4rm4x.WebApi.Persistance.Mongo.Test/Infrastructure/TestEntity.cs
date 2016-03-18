namespace C4rm4x.WebApi.Persistance.Mongo.Test.Infrastructure
{
    public class TestEntity : BaseEntity
    {
        public TestEntity()
            : this(string.Empty)
        {
        }

        public TestEntity(string value)
        {
            Value = value;
        }

        public string Value { get; set; }
    }
}
