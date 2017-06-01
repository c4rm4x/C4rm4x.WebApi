namespace C4rm4x.WebApi.Persistance.EF.Test.Infrastructure
{
    public class TestTable
    {
        public TestTable()
            : this(string.Empty)
        { }

        public TestTable(string value)
        {
            Value = value;
        }

        public int Id { get; set; }

        public string Value { get; set; }
    }
}
