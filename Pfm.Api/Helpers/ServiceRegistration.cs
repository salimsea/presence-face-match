using System;
using Pfm.Core.Interfaces;
using Pfm.Core.Repositories;

namespace Pfm.Api.Helpers
{
	public static class ServiceRegistration
	{
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IPresensi, PresensiRepository>();
        }
    }
}

