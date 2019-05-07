#region USING DIRECTIVES

using System;
using System.Threading.Tasks;

using Discord.Commands;
using Discord.WebSocket;

using Microsoft.Extensions.Configuration;

#endregion USING DIRECTIVES

namespace Kiosk.Core.Services {

  public class CommandHandlingService {
    private DiscordShardedClient Client { get; }
    private CommandService Commands { get; }
    private IServiceProvider Provider { get; }
    private IConfiguration Configuration { get; }
    private Shard Shard { get; set; }

    public CommandHandlingService(DiscordShardedClient client, CommandService commands, IConfiguration config, IServiceProvider provider, Shard shard) {
      Client = client;
      Commands = commands;
      Configuration = config;
      Provider = provider;
      Shard = shard;

      this.Shard.Discord.MessageReceived += HandleCommands;
    }

    private async Task HandleCommands(SocketMessage msg) {
      if(!(msg is SocketUserMessage m) || msg.Author.Id == Client.CurrentUser.Id) return; // If message is empty, return void. If message comes from bot, ignore.

      // Set the command context
      var context = new ShardedCommandContext(this.Client, m);

      int argPos = 0;

      if(m.HasStringPrefix(Configuration["prefix"], ref argPos) || m.HasMentionPrefix(Client.CurrentUser, ref argPos)) {
        var result = await Commands.ExecuteAsync(context, argPos, Provider);
        if(!result.IsSuccess) {
          await context.Channel.SendMessageAsync(result.ToString());
        }
      }
    }
  }
}