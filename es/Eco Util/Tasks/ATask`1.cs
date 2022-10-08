// Decompiled with JetBrains decompiler
// Type: Eco.Util.Tasks.ATask`1
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using DaveyTree.Threading.Tasks.Schedulers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Eco.Util.Tasks
{
  public abstract class ATask<T> : ITask<T>
  {
    private object m_data;

    protected abstract T Work();

    public Task<T> DoWork() => this.DoWork(TaskCreationOptions.None);

    public Task<T> DoWork(TaskCreationOptions options) => Task.Factory.StartNew<T>((Func<T>) (() => this.Work()), CancellationToken.None, options, StaTaskSchedulerEx.Instance());

    public ATask<T> SetData(object data)
    {
      this.m_data = data;
      return this;
    }

    protected virtual object Data => this.m_data;
  }
}
