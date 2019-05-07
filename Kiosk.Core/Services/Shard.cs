#region USING DIRECTIVES

using System;
using System.Reflection;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Kiosk.Core.Common;

#endregion USING DIRECTIVES

namespace Kiosk.Core.Services {

  public sealed class Shard {
    public IServiceProvider Provider { get; set; }
    public DiscordShardedClient Discord { get; set; }
    public CommandService Commands { get; set; }
    private SharedData SharedData { get; set; }

    public Shard(
        IServiceProvider provider,
        CommandService commands,
        DiscordShardedClient client,
        SharedData shared) {
      Provider = provider;
      Discord = client;
      Commands = commands;
      SharedData = shared;
    }

    public async Task StartAsync()
      => await this.Discord.StartAsync();

    public void Initialize() {
      this.SetupClient();
      this.SetupCommands();
    }

    private void SetupClient() {
      var config = new DiscordSocketConfig {
        AlwaysDownloadUsers = this.SharedData.Settings.DownloadUsers,
        ConnectionTimeout = this.SharedData.Settings.Timeout,
        HandlerTimeout = this.SharedData.Settings.HandlerTimeout,
        DefaultRetryMode = this.SharedData.Settings.RetryMode,
        LogLevel = this.SharedData.Settings.LogSeverity,
        MessageCacheSize = this.SharedData.Settings.MessageCache,
        ShardId = this.SharedData.Settings.ShardCount,
        TotalShards = this.SharedData.Settings.ShardCount,
        LargeThreshold = this.SharedData.Settings.Threshhold
      };

      this.Discord = new DiscordShardedClient(config);
      this.Discord.LoginAsync(TokenType.Bot, this.SharedData.Settings.DiscordToken);
    }

    private void SetupCommands() {

      this.Commands = new CommandService(config);
    }
  }
}