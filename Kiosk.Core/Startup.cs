#region USING DIRECTIVES

using Discord;
using Discord.WebSocket;
using Discord.Commands;

using Kiosk.Core.Common;
using Kiosk.Core.Services;

using Newtonsoft.Json;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

#endregion USING DIRECTIVES

namespace Kiosk.Core
{
    public sealed class Startup
    {
        public SharedData Shared { get; private set; }
        public IConfigurationRoot Configuration { get; private set; }

        public Startup()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("Resources/AppSettings.json")
                .Build();
            Configuration = configuration;
        }

        public static async Task RunAsync()
        {
            var startup = new Startup();
            await startup.Run();
        }

        private async Task Run()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            var provider = services.BuildServiceProvider();

            await provider.GetRequiredService<Shard>().StartAsync();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<Shard>()
                    .AddSingleton<CommandHandlingService>()
                    .AddSingleton(Configuration);
        }
    }
}
