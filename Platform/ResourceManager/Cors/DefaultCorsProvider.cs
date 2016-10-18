namespace ResourceManager.Cors
{
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Cors;
    using System.Web.Http.Cors;

    public class DefaultCorsProvider : ICorsPolicyProvider
    {
        public Task<CorsPolicy> GetCorsPolicyAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            CorsPolicy policy = new CorsPolicy
            {
                AllowAnyOrigin = true,
                AllowAnyHeader = true,
                AllowAnyMethod = true
            };
            return Task.FromResult(policy);
        }
    }
}