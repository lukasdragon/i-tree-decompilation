// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.EventPublisher
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using Eco.Domain.v6;
using i_Tree_Eco_v6.Events;
using i_Tree_Eco_v6.SAS;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace i_Tree_Eco_v6
{
  internal static class EventPublisher
  {
    private static readonly IDictionary<Type, Delegate> Subscribers = (IDictionary<Type, Delegate>) new Dictionary<Type, Delegate>();

    public static void Publish<TEventArgs>(TEventArgs args, Control sender) where TEventArgs : EventArgs
    {
      Delegate @delegate;
      lock (EventPublisher.Subscribers)
      {
        if (!EventPublisher.Subscribers.TryGetValue(typeof (TEventArgs), out @delegate))
          return;
      }
      if ((object) @delegate == null)
        return;
      foreach (EventHandler<TEventArgs> invocation in @delegate.GetInvocationList())
      {
        if (invocation.Target != sender)
        {
          if (invocation.Target is Control target && target.InvokeRequired)
            target.Invoke((Delegate) invocation, (object) sender, (object) args);
          else
            invocation((object) sender, args);
        }
      }
    }

    public static bool Register<TEventArgs>(EventHandler<TEventArgs> handler) where TEventArgs : EventArgs
    {
      if (handler.Target is Control target && handler != null)
      {
        lock (EventPublisher.Subscribers)
        {
          if (!EventPublisher.Subscribers.ContainsKey(typeof (TEventArgs)))
            EventPublisher.Subscribers[typeof (TEventArgs)] = (Delegate) null;
          target.Disposed += (EventHandler) ((sender, args) => EventPublisher.Unregister<TEventArgs>(handler));
          EventPublisher.Subscribers[typeof (TEventArgs)] = Delegate.Combine(EventPublisher.Subscribers[typeof (TEventArgs)], (Delegate) handler);
        }
      }
      return target != null;
    }

    public static bool Unregister<TEventArgs>(EventHandler<TEventArgs> handler) where TEventArgs : EventArgs
    {
      lock (EventPublisher.Subscribers)
      {
        if (!EventPublisher.Subscribers.ContainsKey(typeof (TEventArgs)))
          return false;
        EventPublisher.Subscribers[typeof (TEventArgs)] = Delegate.Remove(EventPublisher.Subscribers[typeof (TEventArgs)], (Delegate) handler);
      }
      return true;
    }

    internal static void Publish<T>(
      EntityUpdated<T> entityUpdated,
      SynonymOrCultivarForm synonymOrCultivarForm)
      where T : Entity
    {
      throw new NotImplementedException();
    }
  }
}
