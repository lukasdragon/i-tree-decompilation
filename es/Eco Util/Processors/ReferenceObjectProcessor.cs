// Decompiled with JetBrains decompiler
// Type: Eco.Util.Processors.ReferenceObjectProcessor
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using Eco.Domain.DTO.v6;
using Eco.Domain.v6;
using System;

namespace Eco.Util.Processors
{
  public class ReferenceObjectProcessor : Processor
  {
    public ReferenceObjectProcessor(Updater updater)
      : base(updater)
    {
    }

    public void Create(ReferenceObjectDTO dto, Plot p)
    {
      if (this.Updater.Session.Get<ReferenceObject>((object) dto.Guid) != null)
        return;
      ReferenceObject ro = new ReferenceObject()
      {
        Plot = p
      };
      this.UpdateRefObject(dto, ro);
      this.Save(dto, ro);
    }

    private void Save(ReferenceObjectDTO dto, ReferenceObject ro) => this.Updater.Session.Save((object) ro, (object) dto.Guid);

    public void Update(ReferenceObjectDTO dto, Plot p)
    {
      ReferenceObject referenceObject1 = this.Updater.Session.Get<ReferenceObject>((object) dto.Guid);
      if (referenceObject1 == null)
      {
        ReferenceObject referenceObject2 = new ReferenceObject()
        {
          Plot = p
        };
        this.UpdateRefObject(dto, referenceObject2);
        switch (this.Updater.ResolveConflict(Conflict.EntityDeleted, (Entity) referenceObject2, (Entity) null, dto.Revision))
        {
          case Resolution.UseTheirs:
            this.Save(dto, referenceObject2);
            break;
        }
      }
      else if (referenceObject1.Revision != dto.Revision)
      {
        ReferenceObject referenceObject3 = new ReferenceObject();
        referenceObject3.Plot = p;
        referenceObject3.Revision = referenceObject1.Revision;
        ReferenceObject referenceObject4 = referenceObject3;
        this.UpdateRefObject(dto, referenceObject4);
        switch (this.Updater.ResolveConflict(Conflict.RevisionMismatch, (Entity) referenceObject4, (Entity) referenceObject1, dto.Revision))
        {
          case Resolution.UseTheirs:
            this.Updater.Session.Delete((object) referenceObject1);
            this.Updater.Session.Flush();
            this.Save(dto, referenceObject4);
            break;
        }
      }
      else
      {
        if ((dto.State & State.Modified) == State.Existing)
          return;
        this.UpdateRefObject(dto, referenceObject1);
        this.Updater.Session.Update((object) referenceObject1);
      }
    }

    public void Delete(ReferenceObjectDTO dto) => this.DeleteEntity<ReferenceObject>(dto.Guid);

    private void UpdateRefObject(ReferenceObjectDTO dto, ReferenceObject ro)
    {
      ro.Direction = dto.Direction.GetValueOrDefault(-1);
      ro.Distance = dto.Distance.GetValueOrDefault(-1f);
      ro.DBH = dto.DBH.GetValueOrDefault(-1f);
      ro.Notes = dto.Notes;
      ro.Object = (ReferenceObjectType) dto.Object.GetValueOrDefault(-1);
      ro.Revision = Math.Max(ro.Revision, dto.Revision) + 1;
    }
  }
}
