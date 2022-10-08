// Decompiled with JetBrains decompiler
// Type: <CrtImplementationDetails>.ModuleUninitializer
// Assembly: UFOREWrapper, Version=1.0.5750.15158, Culture=neutral, PublicKeyToken=null
// MVID: A0E9D70D-C711-40C2-8444-67229D4F47C1
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\UFOREWrapper.dll

using \u003CCrtImplementationDetails\u003E;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Security;
using System.Threading;

namespace \u003CCrtImplementationDetails\u003E
{
  internal class ModuleUninitializer : Stack
  {
    private static object @lock = new object();
    internal static ModuleUninitializer _ModuleUninitializer = new ModuleUninitializer();

    [SecuritySafeCritical]
    internal void AddHandler(EventHandler handler)
    {
      bool lockTaken = false;
      RuntimeHelpers.PrepareConstrainedRegions();
      try
      {
        RuntimeHelpers.PrepareConstrainedRegions();
        Monitor.Enter(ModuleUninitializer.@lock, ref lockTaken);
        RuntimeHelpers.PrepareDelegate((Delegate) handler);
        this.Push((object) handler);
      }
      finally
      {
        if (lockTaken)
          Monitor.Exit(ModuleUninitializer.@lock);
      }
    }

    [SecurityCritical]
    static ModuleUninitializer()
    {
    }

    [SecuritySafeCritical]
    private ModuleUninitializer()
    {
      EventHandler eventHandler = new EventHandler(this.SingletonDomainUnload);
      AppDomain.CurrentDomain.DomainUnload += eventHandler;
      AppDomain.CurrentDomain.ProcessExit += eventHandler;
    }

    [PrePrepareMethod]
    [SecurityCritical]
    private void SingletonDomainUnload(object source, EventArgs arguments)
    {
      bool lockTaken = false;
      RuntimeHelpers.PrepareConstrainedRegions();
      try
      {
        RuntimeHelpers.PrepareConstrainedRegions();
        Monitor.Enter(ModuleUninitializer.@lock, ref lockTaken);
        foreach (EventHandler eventHandler in (Stack) this)
          eventHandler(source, arguments);
      }
      finally
      {
        if (lockTaken)
          Monitor.Exit(ModuleUninitializer.@lock);
      }
    }
  }
}
