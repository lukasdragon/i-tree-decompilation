// Decompiled with JetBrains decompiler
// Type: Eco.Util.Tasks.ATask
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using DaveyTree.Threading.Tasks.Schedulers;
using System.Threading;
using System.Threading.Tasks;

namespace Eco.Util.Tasks
{
  public abstract class ATask : ITask
  {
    protected abstract void Work();

    public Task DoWork() => Task.Factory.StartNew((System.Action) (() => this.Work()), CancellationToken.None, TaskCreationOptions.None, StaTaskSchedulerEx.Instance());

    public virtual object Data { get; set; }
  }
}
