using C4rm4x.Tools.Utilities;
using Microsoft.Owin;
using System;
using System.Collections.Generic;

namespace C4rm4x.WebApi.TestUtilities.Acceptance.Internal
{
    internal class MiddlewareContainer : List<Type>
    {
        public MiddlewareContainer(params Type[] middlewares)
        {
            foreach (var middleware in middlewares)
                middleware.Is<OwinMiddleware>();

            this.AddRange(middlewares);
        }
    }
}
