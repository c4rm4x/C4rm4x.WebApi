#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;
using System;
using System.Configuration;
using System.Linq;

#endregion

namespace C4rm4x.WebApi.Persistance.Document.Test.Infrastructure
{
    [TestClass]
    public abstract class BasePersistenceFixture <T>
        : IntegrationFixture<T>
        where T : class
    {
        protected const string TestDatabase = "Test";

        [TestCleanup]
        public override void Cleanup()
        {
            var result = Client.DeleteDatabaseAsync(Database.SelfLink).Result;

            base.Cleanup();
        }

        protected override void RegisterDependencies(
            Container container, 
            Lifestyle lifeStyle)
        {
            container.Register<IDocumentClient>(
                () => new DocumentClient(new Uri(ServiceEndPoint), AuthKey), lifeStyle);            

            base.RegisterDependencies(container, lifeStyle);
        }

        private string ServiceEndPoint => GetSettings("DocumentDb.Settings.ServiceEndPoint");

        private string AuthKey => GetSettings("DocumentDb.Settings.AuthKey");

        private static string GetSettings(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        protected IDocumentClient Client
        {
            get { return GetInstance<IDocumentClient>(); }
        }

        protected Database Database
        {
            get
            {
                return Client.CreateDatabaseQuery().AsEnumerable().FirstOrDefault(d => d.Id == TestDatabase);
            }
        }
    }
}
