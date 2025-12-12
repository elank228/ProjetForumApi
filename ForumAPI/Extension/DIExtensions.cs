using ForumApi.Repositories;
using ForumApi.Repositories.Base;

namespace ForumApi.Extensions
{
    public static class DIExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            // services.AddTransient<IBlobStorageService, BlobStorageService>();

            return services;
        }
    }
}