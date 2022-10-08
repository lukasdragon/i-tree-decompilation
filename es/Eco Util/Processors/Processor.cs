// Decompiled with JetBrains decompiler
// Type: Eco.Util.Processors.Processor
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using Eco.Domain.v6;
using System;

namespace Eco.Util.Processors
{
  public abstract class Processor
  {
    private Updater _updater;

    public Processor(Updater updater) => this._updater = updater;

    protected void DeleteEntity<T>(Guid? g) where T : Entity
    {
      T obj = this._updater.Session.Get<T>((object) g);
      if ((object) obj == null)
        return;
      this._updater.Session.Delete((object) obj);
    }

    protected Updater Updater => this._updater;
  }
}
