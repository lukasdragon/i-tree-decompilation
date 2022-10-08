// Decompiled with JetBrains decompiler
// Type: Eco.Util.TaskManager
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using DaveyTree.Threading.Tasks.Schedulers;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Eco.Util
{
  public class TaskManager
  {
    private List<Task> m_tasks;
    private WaitCursor m_wc;
    private TaskScheduler m_scheduler;

    public TaskManager(WaitCursor wc)
      : this()
    {
      this.m_wc = wc;
    }

    public TaskManager()
    {
      this.m_tasks = new List<Task>();
      this.m_scheduler = StaTaskSchedulerEx.Instance();
    }

    public TaskManager Add(params Task[] tasks)
    {
      if (tasks == null || tasks.Length == 0)
        throw new ArgumentException(nameof (tasks));
      int num = 0;
      lock (this.m_tasks)
      {
        num = this.m_tasks.Count;
        foreach (Task task in tasks)
        {
          if (task != null)
            this.m_tasks.Add(task);
        }
      }
      if (num == 0 && this.m_wc != null)
        Task.Factory.StartNew((System.Action) (() => this.m_wc.Show()), CancellationToken.None, TaskCreationOptions.None, this.m_scheduler);
      foreach (Task task1 in tasks)
      {
        Task t = task1;
        if (t != null)
          t.ContinueWith((System.Action<Task>) (task => this.TaskCompleted(t)), this.m_scheduler);
      }
      return this;
    }

    private void TaskCompleted(Task t)
    {
      int num = 0;
      lock (this.m_tasks)
      {
        this.m_tasks.Remove(t);
        num = this.m_tasks.Count;
      }
      if (num != 0 || this.m_wc == null)
        return;
      Task.Factory.StartNew((System.Action) (() => this.m_wc.Hide()), CancellationToken.None, TaskCreationOptions.None, this.m_scheduler);
    }

    public Task WhenAll()
    {
      Task[] array;
      lock (this.m_tasks)
        array = this.m_tasks.ToArray();
      Task task = Task.WhenAll(array);
      this.Add(task);
      return task;
    }
  }
}
