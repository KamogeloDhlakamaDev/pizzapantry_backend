using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities.Mongo;
using Microsoft.Extensions.Options;
using pizzapantry_backend.Application.Interfaces;

namespace pizzapantry_backend.WebApi
{

    public static class ServiceConfigurator
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            Microsoft.Extensions.Configuration.ConfigurationManager configurationManager = new Microsoft.Extensions.Configuration.ConfigurationManager();

            // Add your service configurations here

            services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddSingleton<IMongoDatabaseSettings>(sp => sp.GetRequiredService<IOptions<MongoDatabaseSettings>>().Value);

            services.AddSingleton<IInventoryDBService>();

            //Metrics setup
            services.AddMetrics();
        }
    }

}