// Decompiled with JetBrains decompiler
// Type: Eco.Util.Processors.PlotLandUseProcessor
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using Eco.Domain.DTO.v6;
using Eco.Domain.v6;
using System;

namespace Eco.Util.Processors
{
  public class PlotLandUseProcessor : Processor
  {
    public PlotLandUseProcessor(Updater updater)
      : base(updater)
    {
    }

    public void Create(PlotLandUseDTO dto, Plot p)
    {
      if (this.Updater.Session.Get<PlotLandUse>((object) dto.Guid) != null)
        return;
      PlotLandUse plu = new PlotLandUse()
      {
        Plot = p
      };
      this.UpdateEntity(dto, plu);
      this.Save(dto, plu);
    }

    private void Save(PlotLandUseDTO dto, PlotLandUse plu) => this.Updater.Session.Save((object) plu, (object) dto.Guid);

    public void Update(PlotLandUseDTO dto, Plot p)
    {
      PlotLandUse plotLandUse1 = this.Updater.Session.Get<PlotLandUse>((object) dto.Guid);
      if (plotLandUse1 == null)
      {
        PlotLandUse plotLandUse2 = new PlotLandUse()
        {
          Plot = p
        };
        this.UpdateEntity(dto, plotLandUse2);
        switch (this.Updater.ResolveConflict(Conflict.EntityDeleted, (Entity) plotLandUse2, (Entity) null, dto.Revision))
        {
          case Resolution.UseTheirs:
            this.Save(dto, plotLandUse2);
            break;
        }
      }
      else if (plotLandUse1.Revision != dto.Revision)
      {
        PlotLandUse plotLandUse3 = new PlotLandUse();
        plotLandUse3.Plot = p;
        plotLandUse3.Revision = plotLandUse1.Revision;
        PlotLandUse plotLandUse4 = plotLandUse3;
        this.UpdateEntity(dto, plotLandUse4);
        switch (this.Updater.ResolveConflict(Conflict.RevisionMismatch, (Entity) plotLandUse4, (Entity) plotLandUse1, dto.Revision))
        {
          case Resolution.UseTheirs:
            this.Updater.Session.Delete((object) plotLandUse1);
            this.Updater.Session.Flush();
            this.Save(dto, plotLandUse4);
            break;
        }
      }
      else
      {
        if ((dto.State & State.Modified) == State.Existing)
          return;
        this.UpdateEntity(dto, plotLandUse1);
        this.Updater.Session.Update((object) plotLandUse1);
      }
    }

    public void Delete(PlotLandUseDTO dto) => this.DeleteEntity<PlotLandUse>(dto.Guid);

    private void UpdateEntity(PlotLandUseDTO dto, PlotLandUse plu)
    {
      plu.PercentOfPlot = dto.PercentOfPlot;
      plu.Revision = Math.Max(plu.Revision, dto.Revision) + 1;
      plu.LandUse = this.Updater.Session.Load<LandUse>((object) dto.LandUse);
    }
  }
}
