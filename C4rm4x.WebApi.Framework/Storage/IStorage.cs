#region Using

using System;
using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebApi.Framework.Storage
{
    /// <summary>
    /// Service responsible to upload 
    /// </summary>
    public interface IStorage
    {
        /// <summary>
        /// Uploads the content into the storage
        /// </summary>
        /// <param name="content">Asset's content</param>
        /// <param name="name">Asset's name (optional)</param>
        /// <returns>The location of the uploaded resource</returns>
        Task<Uri> UploadAsync(            
            byte[] content,
            string name = null);
    }
}
