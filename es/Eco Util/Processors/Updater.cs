// Decompiled with JetBrains decompiler
// Type: Eco.Util.Processors.Updater
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using Eco.Domain.v6;
using NHibernate;
using System;

namespace Eco.Util.Processors
{
  public abstract class Updater
  {
    private ISession _session;
    private ISessionFactory _sf;

    public event EventHandler<ConflictEventArgs> Conflicted;

    public Updater(ISessionFactory sf)
    {
      this._sf = sf;
      this.Status = UpdateStatus.Normal;
    }

    protected virtual void OnConflicted(ConflictEventArgs e)
    {
      EventHandler<ConflictEventArgs> conflicted = this.Conflicted;
      if (conflicted != null)
        conflicted((object) this, e);
      if (e.Resolution != Resolution.Abort)
        return;
      this.Status = UpdateStatus.Aborted;
    }

    public UpdateStatus Status { get; protected set; }

    public ISession Session
    {
      get
      {
        if (this._session == null)
          this._session = this._sf.OpenSession();
        return this._session;
      }
    }

    public Resolution ResolveConflict(
      Conflict conflict,
      Entity theirs,
      Entity mine,
      int theirRev)
    {
      ConflictEventArgs e = new ConflictEventArgs(conflict, theirs, mine, theirRev);
      this.OnConflicted(e);
      return e.Resolution;
    }

    public abstract PlotProcessor PlotProcessor { get; }

    public abstract PlotLandUseProcessor PlotLandUseProcessor { get; }

    public abstract PlotGroundCoverProcessor PlotGroundCoverProcessor { get; }

    public abstract ReferenceObjectProcessor ReferenceObjectProcessor { get; }

    public abstract ShrubProcessor ShrubProcessor { get; }

    public abstract TreeProcessor TreeProcessor { get; }

    public abstract BuildingProcessor BuildingProcessor { get; }

    public abstract StemProcessor StemProcessor { get; }
  }
}
