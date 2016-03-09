#region Using

using System.Web.Http;
using C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Contracts;

#endregion

namespace C4rm4x.WebApi.Monitoring.ServiceStatus.Controllers.Services
{
    internal interface IServiceStatusRequestHandler
    {
        IHttpActionResult Handle(CheckHealthRequest request);
    }
}