// Decompiled with JetBrains decompiler
// Type: Eco.Util.Processors.ShrubProcessor
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using Eco.Domain.DTO.v6;
using Eco.Domain.v6;
using System;

namespace Eco.Util.Processors
{
  public class ShrubProcessor : Processor
  {
    public ShrubProcessor(Updater updater)
      : base(updater)
    {
    }

    public void Create(ShrubDTO dto, Plot p)
    {
      if (this.Updater.Session.Get<Shrub>((object) dto.Guid) != null)
        return;
      Shrub s = new Shrub() { Plot = p };
      this.UpdateShrub(dto, s);
      this.Save(dto, s);
    }

    private void Save(ShrubDTO dto, Shrub s) => this.Updater.Session.Save((object) s, (object) dto.Guid);

    public void Update(ShrubDTO dto, Plot p)
    {
      Shrub shrub1 = this.Updater.Session.Get<Shrub>((object) dto.Guid);
      if (shrub1 == null)
      {
        Shrub shrub2 = new Shrub() { Plot = p };
        this.UpdateShrub(dto, shrub2);
        switch (this.Updater.ResolveConflict(Conflict.EntityDeleted, (Entity) shrub2, (Entity) null, dto.Revision))
        {
          case Resolution.UseTheirs:
            this.Save(dto, shrub2);
            break;
        }
      }
      else if (shrub1.Revision != dto.Revision)
      {
        Shrub shrub3 = new Shrub();
        shrub3.Plot = p;
        shrub3.Revision = shrub1.Revision;
        Shrub shrub4 = shrub3;
        this.UpdateShrub(dto, shrub4);
        switch (this.Updater.ResolveConflict(Conflict.RevisionMismatch, (Entity) shrub4, (Entity) shrub1, dto.Revision))
        {
          case Resolution.UseTheirs:
            this.Updater.Session.Delete((object) shrub1);
            this.Updater.Session.Flush();
            this.Save(dto, shrub4);
            break;
        }
      }
      else
      {
        if ((dto.State & State.Modified) == State.Existing)
          return;
        this.UpdateShrub(dto, shrub1);
        this.Updater.Session.Update((object) shrub1);
      }
    }

    public void Delete(ShrubDTO dto) => this.DeleteEntity<Shrub>(dto.Guid);

    private void UpdateShrub(ShrubDTO dto, Shrub s)
    {
      s.Id = dto.Id;
      s.Species = dto.Species;
      s.Height = dto.Height.GetValueOrDefault(-1f);
      s.PercentMissing = (PctMidRange) dto.PercentMissing.GetValueOrDefault(-1);
      s.PercentOfShrubArea = dto.PercentOfShrubArea.GetValueOrDefault(-1);
      s.Comments = dto.Comments;
      s.Revision = Math.Max(s.Revision, dto.Revision) + 1;
    }
  }
}
