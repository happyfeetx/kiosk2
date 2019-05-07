#region USING DIRECTIVES

using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Kiosk.Core.Common;
using Kiosk.Core.Services;

using Discord.Commands;
using Discord.WebSocket;

using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;

#endregion USING DIRECTIVES

namespace Kiosk.Core {

  internal static class Kiosk {
    public static string ApplicationName { get; }
    public static string ApplicationVersion { get; }
    public static string ApplicationAuthor { get; }
    public static int ApplicationRevision { get; }

    private static AppSettings Settings { get; set; }
    private static SharedData Shared { get; set; }
    private static IServiceProvider Provider { get; set; }
    private static DiscordShardedClient Client { get; set; }
    private static CommandService Commands { get; }

    static Kiosk() {
      Settings = AppSettings.Default;
      Shared = new SharedData();
      ApplicationAuthor = "Wonderfull Group";
      ApplicationName = "Kiosk.Core";
      ApplicationVersion = "v2.0.1";
      ApplicationRevision = 2;
    }

    private static async Task Main() {
      try {
        PrintBuildInformation();

        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.SystemDefault;

        await LoadConfigurationAsync();

        try {
          await Task.Delay(Timeout.Infinite, Shared.MainLoopCts.Token);
        } catch(TaskCanceledException) {
          Console.WriteLine("\rShutdown signal received!          ");
        }
      } catch(Exception e) {
        Console.WriteLine($"{e.GetType()} :\n{e.Message}            ");
        if(!(e.InnerException is null)) {
          Console.WriteLine($"{e.GetType()} :\n{e.InnerException.Message}");
        }
      }

      Console.WriteLine("\rShutting down!                             ");
      Console.ReadKey();
    }

    private static void PrintBuildInformation() {
      var a = Assembly.GetExecutingAssembly();
      var fvi = FileVersionInfo.GetVersionInfo(a.Location);

      Console.WriteLine($"{ApplicationName} {ApplicationVersion} [{ApplicationRevision}]\n- {ApplicationAuthor}");
      Console.WriteLine();
    }

    private static async Task LoadConfigurationAsync() {
      Console.WriteLine("\r[1/3] Loading configuration...");

      string json = "{}";
      var utf8 = new UTF8Encoding(false);
      var fi = new FileInfo("appsettings.json");
      if(!fi.Exists) {
        Console.WriteLine("\rLoading configuration failed!");
        Console.WriteLine("\rConfiguration file could not be found. A new one will be created at: ");
        Console.WriteLine(fi.ToString());
        Console.WriteLine("\rPlease fill in with desired values and re-run the program.");

        json = JsonConvert.SerializeObject(AppSettings.Default, Formatting.Indented);
        using(FileStream fs = fi.Create()) {
          using(StreamWriter sw = new StreamWriter(fs, utf8)) {
            await sw.WriteAsync(json);
            await sw.FlushAsync();
          }
        }

        throw new IOException("Configuration file not found!");
      }

      using(FileStream fs = fi.OpenRead()) {
        using(StreamReader sr = new StreamReader(fs, utf8)) {
          json = await sr.ReadToEndAsync();
        }
      }

      Settings = JsonConvert.DeserializeObject<AppSettings>(json);
    }

    private static async Task CreateShards() {
      var provider = new ServiceCollection()
        .AddSingleton<Shard>()
        .AddSingleton<CommandHandlingService>()
        .BuildServiceProvider();

      Console.WriteLine($"\r[2/3] Loading {Shared.Settings.ShardCount} shards...");

      for(int i = 0; i < Settings.ShardCount; i++) {
        var shard = new Shard(provider, Commands, Client, Shared);
      }
    }
  }
}