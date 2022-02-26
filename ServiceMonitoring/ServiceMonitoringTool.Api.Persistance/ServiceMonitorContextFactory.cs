using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ServiceMonitoringTool.Api.Persistance
{
    public class ServiceMonitorContextFactory : IDesignTimeDbContextFactory<ServiceMonitorContext>
    {
        public ServiceMonitorContext CreateDbContext(string[] args){
            var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var config = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
            .AddJsonFile("appsettings.json",optional:false)
            .AddJsonFile($"appsettings.{envName}.json",optional:false)
            .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ServiceMonitorContext>();
            optionsBuilder.UseSqlServer(config["DataConnection:Database"]);

            return new ServiceMonitorContext(optionsBuilder.Options);
        }
    }
}