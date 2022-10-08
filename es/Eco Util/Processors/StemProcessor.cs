// Decompiled with JetBrains decompiler
// Type: Eco.Util.Processors.StemProcessor
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using Eco.Domain.DTO.v6;
using Eco.Domain.v6;
using System;

namespace Eco.Util.Processors
{
  public class StemProcessor : Processor
  {
    public StemProcessor(Updater updater)
      : base(updater)
    {
    }

    public void Create(StemDTO dto, Tree t)
    {
      if (this.Updater.Session.Get<Stem>((object) dto.Guid) != null)
        return;
      Stem s = new Stem() { Tree = t };
      this.UpdateStem(dto, s);
      this.Save(dto, s);
    }

    private void Save(StemDTO dto, Stem s) => this.Updater.Session.Save((object) s, (object) dto.Guid);

    public void Update(StemDTO dto, Tree t)
    {
      Stem stem1 = this.Updater.Session.Get<Stem>((object) dto.Guid);
      if (stem1 == null)
      {
        Stem stem2 = new Stem() { Tree = t };
        this.UpdateStem(dto, stem2);
        switch (this.Updater.ResolveConflict(Conflict.EntityDeleted, (Entity) stem2, (Entity) null, dto.Revision))
        {
          case Resolution.UseTheirs:
            this.Save(dto, stem2);
            break;
        }
      }
      else if (stem1.Revision != dto.Revision)
      {
        Stem stem3 = new Stem();
        stem3.Tree = t;
        stem3.Revision = stem1.Revision;
        Stem stem4 = stem3;
        this.UpdateStem(dto, stem4);
        switch (this.Updater.ResolveConflict(Conflict.RevisionMismatch, (Entity) stem4, (Entity) stem1, dto.Revision))
        {
          case Resolution.UseTheirs:
            this.Updater.Session.Delete((object) stem1);
            this.Updater.Session.Flush();
            this.Save(dto, stem4);
            break;
        }
      }
      else
      {
        if ((dto.State & State.Modified) == State.Existing)
          return;
        this.UpdateStem(dto, stem1);
        this.Updater.Session.Update((object) stem1);
      }
    }

    public void Delete(StemDTO dto) => this.DeleteEntity<Stem>(dto.Guid);

    private void UpdateStem(StemDTO dto, Stem s)
    {
      s.Id = dto.Id;
      s.Diameter = dto.Diameter.GetValueOrDefault(-1.0);
      s.DiameterHeight = dto.DiameterHeight.GetValueOrDefault(-1.0);
      s.Measured = dto.Measured.GetValueOrDefault(true);
      s.Revision = Math.Max(s.Revision, dto.Revision) + 1;
    }
  }
}
