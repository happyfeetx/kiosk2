#region USING DIRECTIVES

using Discord;
using Discord.WebSocket;
using Discord.Commands;

using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;

using Kiosk.Core.Common;

#endregion USING DIRECTIVES

namespace Kiosk.Core.Services
{
    public class CommandHandlingService
    {
        private DiscordShardedClient Client { get; }
        private CommandService Commands { get; }
        private IServiceProvider Provider { get; }
        private IConfiguration Configuration { get; }

        public CommandHandlingService(DiscordShardedClient client, CommandService commands, IConfiguration config, IServiceProvider provider)
        {
            Client = client;
            Commands = commands;
            Configuration = config;
            Provider = provider;

            this.Client.MessageReceived += HandleCommands;
        }

        private async Task HandleCommands(SocketMessage msg)
        {
            var m = msg as SocketUserMessage;
            if (m == null || msg.Author.Id == Client.CurrentUser.Id) return; // If message is empty, return void. If message comes from bot, ignore.


            // Set the command context
            var context = new ShardedCommandContext(this.Client, m);

            int argPos = 0;

            if (m.HasStringPrefix(Configuration["prefix"], ref argPos) || m.HasMentionPrefix(Client.CurrentUser, ref argPos))
            {
                var result = await Commands.ExecuteAsync(context, argPos, Provider);
                if (!result.IsSuccess)
                {
                    await context.Channel.SendMessageAsync(result.ToString());
                }
            }
        }
    }
}
