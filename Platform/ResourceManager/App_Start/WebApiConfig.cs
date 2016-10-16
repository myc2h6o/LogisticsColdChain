
namespace ResourceManager
{
    using Models;
    using Microsoft.Restier.Providers.EntityFramework;
    using Microsoft.Restier.Publishers.OData;
    using System.Linq;
    using System.Web.Http;
    using System.Web.OData.Extensions;
    using Microsoft.Restier.Publishers.OData.Batch;

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
