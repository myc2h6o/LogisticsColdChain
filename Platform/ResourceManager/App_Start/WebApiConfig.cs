
namespace ResourceManager
{
    using Microsoft.Restier.Providers.EntityFramework;
    using Microsoft.Restier.Publishers.OData;
    using Microsoft.Restier.Publishers.OData.Batch;
    using Models;
    using System.Web.Http;
    using System.Web.OData.Extensions;

    public static class WebApiConfig
    {
        public async static void Register(HttpConfiguration config)
        {
            // enable query options for all properties
            config.Filter();
            await config.MapRestierRoute<EntityFrameworkApi<Logistics>>(
                "Logistics",
                "logistics",
                new RestierBatchHandler(GlobalConfiguration.DefaultServer));
        }
    }
}
