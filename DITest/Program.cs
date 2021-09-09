using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace DITest
{
    class Program
    {
        private const int EXITCODE_OK = 0x0;
        private const int EXITCODE_ERROR = 0x1;

        static int Main(string[] args)
        {
            // Serilog.Extensions.Hosting
            // Serilog.Sinks.Console
            // Serilog.Sinks.File
            // Serilog.Settings.AppSettings
            Log.Logger = new LoggerConfiguration()
                         .ReadFrom.AppSettings()
                         .CreateLogger();

            try
            {
                Log.Information("Starting host");
                CreateHostBuilder(args).Build().Run();
                return EXITCODE_OK;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return EXITCODE_ERROR;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                })
                .UseSerilog();
    }
}
