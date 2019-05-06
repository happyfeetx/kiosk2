#region USING DIRECTIVES

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using System;
using System.Reflection;
using System.Threading.Tasks;

#endregion USING DIRECTIVES

namespace Kiosk.Core.Common
{
    public sealed class Shard
    {
        private IServiceProvider Provider { get; }
        private DiscordShardedClient Discord { get; }
        private CommandService Commands { get; }
        private SharedData SharedData { get; }

        public Shard(
            IServiceProvider provider, 
            CommandService commands, 
            SharedData shared)
        {
            var config = new DiscordSocketConfig
            {
                LogLevel = this.SharedData.Settings.LogSeverity,
                DefaultRetryMode = this.SharedData.Settings.RetryMode,
                AlwaysDownloadUsers = this.SharedData.Settings.DownloadUsers,
                HandlerTimeout = this.SharedData.Settings.HandlerTimeout,
                ConnectionTimeout = this.SharedData.Settings.Timeout,
                MessageCacheSize = this.SharedData.Settings.MessageCache,
                TotalShards = this.SharedData.Settings.ShardCount
            };

            Provider = provider;
            Discord = new DiscordShardedClient(config);
            Commands = commands;
            SharedData = shared;
        }

        public async Task StartAsync()
        {
            string Token = this.SharedData.Settings.DiscordToken;
            if (string.IsNullOrWhiteSpace(Token))
            {
                throw new Exception("Please enter your bot's token into the AppSettings.json!");
            }

            await this.Discord.LoginAsync(TokenType.Bot, Token);
            await this.Discord.StartAsync();

            await this.Commands.AddModulesAsync(Assembly.GetEntryAssembly(), Provider);
        }
    }
}
