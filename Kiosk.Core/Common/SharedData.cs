#region USING DIRECTIVES

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

using Kiosk.Core.Services;

#endregion USING DIRECTIVES

namespace Kiosk.Core.Common {

  public sealed class SharedData : IDisposable {
    public CancellationTokenSource MainLoopCts { get; internal set; }
    public ConcurrentDictionary<ulong, int> Messages { get; internal set; }
    public bool ListeningStatus { get; internal set; }
    public bool StatusRotationEnabled { get; internal set; }
    public AppSettings Settings { get; internal set; }

    public IReadOnlyList<Shard> ActiveShards => Shards.AsReadOnly();
    public List<Shard> Shards { get; internal set; }

    public SharedData() {
      this.MainLoopCts = new CancellationTokenSource();
      this.Messages = new ConcurrentDictionary<ulong, int>();
      this.ListeningStatus = true;
      this.StatusRotationEnabled = true;
      this.Settings = AppSettings.Default;
    }

    public void Dispose() {
      this.MainLoopCts.Dispose();
    }
  }
}