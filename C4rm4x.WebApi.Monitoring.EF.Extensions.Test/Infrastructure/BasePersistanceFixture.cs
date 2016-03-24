#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;

#endregion

namespace C4rm4x.WebApi.Monitoring.EF.Test
{
    [TestClass]
    public abstract class BasePersistanceFixture<T>
        : IntegrationFixture<T>
        where T : class
    {
        [TestInitialize]
        public override void Setup()
        {
            base.Setup();

            ExecuteNonSql(Resource.CreateTable);
        }

        protected override void RegisterDependencies(
            Container container,
            Lifestyle lifeStyle)
        {
            container.Register<TestEntities>(lifeStyle);

            base.RegisterDependencies(container, lifeStyle);
        }

        [TestCleanup]
        public override void Cleanup()
        {
            ExecuteNonSql(Resource.DropTable);

            base.Cleanup();
        }

        protected void ExecuteNonSql(string sql)
        {
            TestEntities.Database.ExecuteSqlCommand(sql);
        }

        private TestEntities TestEntities
        {
            get { return GetInstance<TestEntities>(); }
        }
    }
}
