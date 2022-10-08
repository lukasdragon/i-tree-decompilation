// Decompiled with JetBrains decompiler
// Type: Eco.Util.Processors.PlotGroundCoverProcessor
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using Eco.Domain.DTO.v6;
using Eco.Domain.v6;
using System;

namespace Eco.Util.Processors
{
  public class PlotGroundCoverProcessor : Processor
  {
    public PlotGroundCoverProcessor(Updater updater)
      : base(updater)
    {
    }

    public void Create(PlotGroundCoverDTO dto, Plot p)
    {
      if (this.Updater.Session.Get<PlotGroundCover>((object) dto.Guid) != null)
        return;
      PlotGroundCover pgc = new PlotGroundCover()
      {
        Plot = p
      };
      this.UpdateEntity(dto, pgc);
      this.Save(dto, pgc);
    }

    private void Save(PlotGroundCoverDTO dto, PlotGroundCover pgc) => this.Updater.Session.Save((object) pgc, (object) dto.Guid);

    public void Update(PlotGroundCoverDTO dto, Plot p)
    {
      PlotGroundCover plotGroundCover1 = this.Updater.Session.Get<PlotGroundCover>((object) dto.Guid);
      if (plotGroundCover1 == null)
      {
        PlotGroundCover plotGroundCover2 = new PlotGroundCover()
        {
          Plot = p
        };
        this.UpdateEntity(dto, plotGroundCover2);
        switch (this.Updater.ResolveConflict(Conflict.EntityDeleted, (Entity) plotGroundCover2, (Entity) null, dto.Revision))
        {
          case Resolution.UseTheirs:
            this.Save(dto, plotGroundCover2);
            break;
        }
      }
      else if (plotGroundCover1.Revision != dto.Revision)
      {
        PlotGroundCover plotGroundCover3 = new PlotGroundCover();
        plotGroundCover3.Plot = p;
        plotGroundCover3.Revision = plotGroundCover1.Revision;
        PlotGroundCover plotGroundCover4 = plotGroundCover3;
        this.UpdateEntity(dto, plotGroundCover4);
        switch (this.Updater.ResolveConflict(Conflict.RevisionMismatch, (Entity) plotGroundCover4, (Entity) plotGroundCover1, dto.Revision))
        {
          case Resolution.UseTheirs:
            this.Updater.Session.Delete((object) plotGroundCover1);
            this.Updater.Session.Flush();
            this.Save(dto, plotGroundCover4);
            break;
        }
      }
      else
      {
        if ((dto.State & State.Modified) == State.Existing)
          return;
        this.UpdateEntity(dto, plotGroundCover1);
        this.Updater.Session.Update((object) plotGroundCover1);
      }
    }

    public void Delete(PlotGroundCoverDTO dto) => this.DeleteEntity<PlotGroundCover>(dto.Guid);

    private void UpdateEntity(PlotGroundCoverDTO dto, PlotGroundCover pgc)
    {
      pgc.PercentCovered = dto.PercentCovered;
      pgc.GroundCover = dto.GroundCover.HasValue ? this.Updater.Session.Load<GroundCover>((object) dto.GroundCover) : (GroundCover) null;
      pgc.Revision = Math.Max(pgc.Revision, dto.Revision) + 1;
    }
  }
}
