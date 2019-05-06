#region USING DIRECTIVES

using Discord;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

#endregion USING DIRECTIVES

namespace Kiosk.Core.Common
{
    public sealed class AppSettings
    {
        [JsonProperty("database")]
        public string Db { get; private set; }
            [JsonProperty("name")]
            public string DatabaseName { get; private set; }
            [JsonProperty("login")]
            public string DatabaseLogin { get; private set; }
            [JsonProperty("password")]
            public string DatabasePassword { get; private set; }
            [JsonProperty("provider")]
            public DatabaseProvider Provider { get; private set; }
            [JsonProperty("hostname")]
            public string Hostname { get; private set; }
            [JsonProperty("port")]
            public short Port { get; private set; }

        [JsonIgnore]
        public static AppSettings Database => new AppSettings()
        {
            Db = "",
            DatabaseName = "KioskDB",
            DatabaseLogin = "Database",
            DatabasePassword = "",
            Provider = DatabaseProvider.SQLite,
            Hostname = "localhost",
            Port = 5445
        };

        [JsonProperty("discord")]
        public string Discord { get; private set; }
            [JsonProperty("token")]
            public string DiscordToken { get; private set; }
            [JsonProperty("log-severity")]
            public LogSeverity LogSeverity { get; private set; }
            [JsonProperty("log-to-file")]
            public bool LogToFile { get; private set; }
            [JsonProperty("message-cache-size")]
            public int MessageCache { get; private set; }
            [JsonProperty("connection-timeout")]
            public int Timeout { get; private set; }
            [JsonProperty("download-users")]
            public bool DownloadUsers { get; private set; }
            [JsonProperty("log-level")]
            public LogLevel LogLevel { get; private set; }
            [JsonProperty("retry-mode")]
            public RetryMode RetryMode { get; private set; }
            [JsonProperty("shard-count")]
            public int ShardCount { get; private set; }
            [JsonProperty("handler-timeout")]
            public int HandlerTimeout { get; private set; }

        [JsonIgnore]
        public static AppSettings Bot => new AppSettings()
        {
            Discord = "",
            DiscordToken = "<Token here>",
            LogSeverity = LogSeverity.Info,
            LogToFile = false,
            MessageCache = 5000,
            Timeout = 12500,
            DownloadUsers = true,
            LogLevel = LogLevel.Information,
            RetryMode = RetryMode.AlwaysRetry,
            ShardCount = 1,
            HandlerTimeout = 15000
        };
    }

    public enum DatabaseProvider
    {
        PostgreSQL = 0,
        SQLite = 1,
        SQLServer = 2,
        CosmosDB = 3,
        InMemory = 4
    }
}
