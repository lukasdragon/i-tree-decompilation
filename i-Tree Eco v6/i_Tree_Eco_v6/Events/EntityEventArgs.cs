// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Events.EntityEventArgs
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using Eco.Domain.v6;
using System;

namespace i_Tree_Eco_v6.Events
{
  public abstract class EntityEventArgs : EventArgs
  {
    public Guid Guid { get; protected set; }

    public EntityEventArgs(Guid g) => this.Guid = g;

    public EntityEventArgs(Entity entity) => this.Guid = entity.Guid;
  }
}
