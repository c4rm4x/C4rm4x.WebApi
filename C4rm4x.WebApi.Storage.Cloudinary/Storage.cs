#region Using

using C4rm4x.Tools.Utilities;
using C4rm4x.WebApi.Framework.Storage;
using CloudinaryDotNet.Actions;
using System;
using System.IO;
using System.Threading.Tasks;
using CloudinaryClient = CloudinaryDotNet.Cloudinary;

#endregion

namespace C4rm4x.WebApi.Storage.Cloudinary
{
    /// <summary>
    /// Implementation of IStorage using Cloudinary asset mamagement API
    /// </summary>
    public class Storage : IStorage
    {
        private readonly CloudinaryClient _client;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">Instance of Cloudinary client</param>
        public Storage(CloudinaryClient client)
        {
            client.NotNull(nameof(client));

            _client = client;
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

            var result = await _client.UploadAsync(GetParameters(content, name));

            return result
                .EnsureSuccessStatusCode()
                .Uri;
        }

        private ImageUploadParams GetParameters(
            byte[] content,
            string name)
        {
            name = name ?? Guid.NewGuid().ToString().Replace("-", string.Empty);

            return new ImageUploadParams
            {
                File = new FileDescription(name, new MemoryStream(content)),
                Folder = name
            };
        }
    }
}
