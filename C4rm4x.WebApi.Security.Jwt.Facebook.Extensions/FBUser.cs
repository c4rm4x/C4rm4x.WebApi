#region Using

using C4rm4x.Tools.Utilities;
using System;

#endregion

namespace C4rm4x.WebApi.Security.Jwt.Facebook
{
    /// <summary>
    /// FB user information
    /// </summary>
    public class FBUser
    {
        /// <summary>
        /// Gets the user's id
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Gets the user's name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the user's picture url
        /// </summary>
        public Uri Picture { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">User's id</param>
        /// <param name="name">User's name</param>
        /// <param name="pictureUrl">User's picture url</param>
        public FBUser(
            string id,
            string name,
            string pictureUrl)
        {
            Uri locationAsUri = null;

            id.NotNullOrEmpty(nameof(id));
            name.NotNullOrEmpty(nameof(name));
            pictureUrl.NotNullOrEmpty(nameof(pictureUrl));
            pictureUrl.Must(x => Uri.TryCreate(x, UriKind.Absolute, out locationAsUri), "pictureUrl is not a valid URL");

            Id = id;
            Name = name;
            Picture = locationAsUri;
        }
    }
}
