#region Using

using System.Drawing;

#endregion

namespace C4rm4x.WebApi.Storage.Cloudinary.Test
{
    internal static class Extensions
    {
        public static byte[] GetAsByteArray(
            this Bitmap source)
        {
            var converter = new ImageConverter();

            return (byte[])converter.ConvertTo(source, typeof(byte[]));
        }
    }
}
