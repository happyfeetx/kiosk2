#region USING DIRECTIVES

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Collections.Generic;

#endregion USING DIRECTIVES

namespace Kiosk.Core.Common
{
    public sealed class SharedData
    {
        public CancellationTokenSource MainLoopCts { get; internal set; }
        public ConcurrentDictionary<ulong, int> Messages { get; internal set; }
        public AppSettings Settings { get; internal set; }
        public AppSettings DbSettings { get; internal set; }
        public bool ListeningStatus { get; internal set; }
        public bool StatusRotationEnabled { get; internal set; }
        public List<Shard> Shards { get; internal set; }

        public SharedData()
        {
            this.MainLoopCts = new CancellationTokenSource();
            this.Messages = new ConcurrentDictionary<ulong, int>();
            this.Settings = AppSettings.Bot;
            this.DbSettings = AppSettings.Database;
            this.ListeningStatus = true;
            this.StatusRotationEnabled = true;
            this.Shards = new List<Shard>();
        }
    }
}
