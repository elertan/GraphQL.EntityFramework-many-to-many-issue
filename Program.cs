using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace GraphQL.EntityFramework_many_to_many_issue
{
    class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
