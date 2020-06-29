using ChustaSoft.Tools.SecureConfig.TestApi.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChustaSoft.Tools.SecureConfig.TestApi
{
    public class Startup
    {

        public readonly IConfiguration _configuration;


        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            var testApikey = "5357F6B0313A478A9BF901BB37B4A458";
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.SetUpSecureConfig<AppSettings>(_configuration, testApikey);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
