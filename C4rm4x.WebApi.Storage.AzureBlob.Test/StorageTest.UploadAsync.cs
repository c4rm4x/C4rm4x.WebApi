#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage;
using SimpleInjector;
using System.Configuration;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Storage.AzureBlob.Test
{
    public partial class StorageTest
    {
        [TestClass]
        public class StorageUploadAsyncTest :
            IntegrationFixture<Storage>
        {
            protected override void RegisterDependencies(
                Container container,
                Lifestyle lifeStyle)
            {
                base.RegisterDependencies(container, lifeStyle);

                container.Register<IContainerReferenceFactory>(() =>
                    new ContainerReferenceFactory("test"), lifeStyle);
                container.Register(() =>
                    CloudStorageAccount.Parse(ConnectionString), lifeStyle);
            }

            private static string ConnectionString => 
                ConfigurationManager.ConnectionStrings["AzureConnection"].ConnectionString;

            [TestMethod, IntegrationTest]
            public async Task UploadAsync_Uploads_The_Image_Successfully()
            {
                var uri = await _sut.UploadAsync(
                    Resources.COL9_6.GetAsByteArray(),
                    ObjectMother.Create<string>());

                Assert.IsNotNull(uri);
            }
        }
    }
}
