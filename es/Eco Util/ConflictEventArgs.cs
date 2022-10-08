// Decompiled with JetBrains decompiler
// Type: Eco.Util.ConflictEventArgs
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using Eco.Domain.v6;
using System;

namespace Eco.Util
{
  public class ConflictEventArgs : EventArgs
  {
    public ConflictEventArgs(Conflict conflict, Entity theirs, Entity mine, int theirRev)
    {
      this.Conflict = conflict;
      this.Theirs = theirs;
      this.TheirRevision = theirRev;
      this.Mine = mine;
      this.Resolution = Resolution.Abort;
    }

    public Conflict Conflict { get; protected set; }

    public Entity Theirs { get; protected set; }

    public Entity Mine { get; protected set; }

    public int TheirRevision { get; private set; }

    public int MyRevision => this.Mine != null ? this.Mine.Revision : 0;

    public Resolution Resolution { get; set; }
  }
}
