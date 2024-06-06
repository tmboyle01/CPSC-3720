using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using TigerTix.Data;

namespace TigerTix
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = "connection string to db";
            services.AddDbContext<TigerTixContext>(cfg
                => cfg.UseSqlServer(connectionString));
        }
    }

    //public void Configure(IApplicationBuilder app)
    //{
    //    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    //}
}
