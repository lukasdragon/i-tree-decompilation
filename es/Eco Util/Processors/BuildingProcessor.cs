// Decompiled with JetBrains decompiler
// Type: Eco.Util.Processors.BuildingProcessor
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using Eco.Domain.DTO.v6;
using Eco.Domain.v6;
using System;

namespace Eco.Util.Processors
{
  public class BuildingProcessor : Processor
  {
    public BuildingProcessor(Updater updater)
      : base(updater)
    {
    }

    public void Create(BuildingDTO dto, Tree t)
    {
      if (this.Updater.Session.Get<Building>((object) dto.Guid) != null)
        return;
      Building b = new Building() { Tree = t };
      this.UpdateBuilding(dto, b);
      this.Save(dto, b);
    }

    private void Save(BuildingDTO dto, Building b) => this.Updater.Session.Save((object) b, (object) dto.Guid);

    public void Update(BuildingDTO dto, Tree t)
    {
      Building building1 = this.Updater.Session.Get<Building>((object) dto.Guid);
      if (building1 == null)
      {
        Building building2 = new Building()
        {
          Tree = t
        };
        this.UpdateBuilding(dto, building2);
        switch (this.Updater.ResolveConflict(Conflict.EntityDeleted, (Entity) building2, (Entity) null, dto.Revision))
        {
          case Resolution.UseTheirs:
            this.Save(dto, building2);
            break;
        }
      }
      else if (building1.Revision != dto.Revision)
      {
        Building building3 = new Building();
        building3.Tree = t;
        building3.Revision = building1.Revision;
        Building building4 = building3;
        this.UpdateBuilding(dto, building4);
        switch (this.Updater.ResolveConflict(Conflict.RevisionMismatch, (Entity) building4, (Entity) building1, dto.Revision))
        {
          case Resolution.UseTheirs:
            this.Updater.Session.Delete((object) building1);
            this.Updater.Session.Flush();
            this.Save(dto, building4);
            break;
        }
      }
      else
      {
        if ((dto.State & State.Modified) == State.Existing)
          return;
        this.UpdateBuilding(dto, building1);
        this.Updater.Session.Update((object) building1);
      }
    }

    public void Delete(BuildingDTO dto) => this.DeleteEntity<Building>(dto.Guid);

    public void UpdateBuilding(BuildingDTO dto, Building b)
    {
      b.Id = dto.Id;
      b.Direction = dto.Direction.GetValueOrDefault((short) -1);
      b.Distance = dto.Distance.GetValueOrDefault(-1f);
      b.Revision = Math.Max(b.Revision, dto.Revision) + 1;
    }
  }
}
