#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;
using System.Data.Entity;

#endregion

namespace C4rm4x.WebApi.Persistance.EF.Test.Infrastructure
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

        protected EntityState GetEntityState(TestTable entity)
        {
            return TestEntities.Entry(entity).State;
        }

        protected void SetEntityState(TestTable entity, EntityState newState)
        {
            TestEntities.Entry(entity).State = newState;
        }

        private TestEntities TestEntities
        {
            get { return GetInstance<TestEntities>(); }
        }
    }
}
