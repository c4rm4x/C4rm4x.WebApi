#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;
using System.Configuration;
using System.Threading.Tasks;
using CloudinaryAccount = CloudinaryDotNet.Account;
using CloudinaryClient = CloudinaryDotNet.Cloudinary;

#endregion

namespace C4rm4x.WebApi.Storage.Cloudinary.Test
{
    public partial class StorageTest
    {
        [TestClass]
        public class StorageUploadAsyncTest :
            IntegrationFixture<Storage>
        {
            public string Cloud => ConfigurationManager.AppSettings["Clodinary.Cloud"];

            public string ApiKey => ConfigurationManager.AppSettings["Clodinary.ApiKey"];

            public string ApiSecret => ConfigurationManager.AppSettings["Clodinary.ApiSecret"];

            protected override void RegisterDependencies(
                Container container, 
                Lifestyle lifeStyle)
            {
                base.RegisterDependencies(container, lifeStyle);

                container.Register(() =>
                    new CloudinaryClient(
                        new CloudinaryAccount(Cloud, ApiKey, ApiSecret)), 
                    lifeStyle);
            }

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
