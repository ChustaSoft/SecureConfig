using ChustaSoft.Tools.SecureConfig.TestApi.Configuration;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace ChustaSoft.Tools.SecureConfig.TestApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args)
                .Build()
                .EncryptSettings<AppSettings>(false)
                .Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
