#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.Storage;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Storage.AzureBlob
{
    /// <summary>
    /// Implementation of IStorage using Azure Blob
    /// </summary>
    public class Storage : IStorage
    {
        private readonly CloudStorageAccount _cloudStorageAccount;

        private readonly IContainerReferenceFactory _containerReferenceFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cloudStorageAccount">The cloud storage account</param>
        /// <param name="containerReferenceFactory">The container reference factory</param>
        public Storage(
            CloudStorageAccount cloudStorageAccount,
            IContainerReferenceFactory containerReferenceFactory)
        {
            cloudStorageAccount.NotNull(nameof(cloudStorageAccount));
            containerReferenceFactory.NotNull(nameof(containerReferenceFactory));

            _cloudStorageAccount = cloudStorageAccount;
            _containerReferenceFactory = containerReferenceFactory;
        }

        /// <summary>
        /// Uploads the content into the storage
        /// </summary>
        /// <param name="content">Asset's content</param>
        /// <param name="name">Asset's name (optional)</param>
        /// <returns>The location of the uploaded resource</returns>
        public async Task<Uri> UploadAsync(
            byte[] content,
            string name = null)
        {
            content.NotNull(nameof(content));

            var blockBlob = _cloudStorageAccount
                .CreateCloudBlobClient()
                .GetContainerReference(_containerReferenceFactory.Get())
                .GetBlockBlobReference(name ?? Guid.NewGuid().ToString().Replace("-", string.Empty));

            await blockBlob.UploadFromByteArrayAsync(content, 0, content.Length);

            return blockBlob.Uri;
        }
    }
}
