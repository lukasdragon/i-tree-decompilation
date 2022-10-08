// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v6.PlantingSite
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using Eco.Domain.DTO;
using Eco.Domain.DTO.v6;
using System;

namespace Eco.Domain.v6
{
  public class PlantingSite : Entity<PlantingSite>
  {
    public virtual Plot Plot
    {
      get => this.\u003CPlot\u003Ek__BackingField;
      set
      {
        if (this.\u003CPlot\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (Plot));
        this.\u003CPlot\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Plot);
      }
    }

    public virtual int Id
    {
      get => this.\u003CId\u003Ek__BackingField;
      set
      {
        if (this.\u003CId\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (Id));
        this.\u003CId\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Id);
      }
    }

    public virtual PlantingSiteType PlantingSiteType
    {
      get => this.\u003CPlantingSiteType\u003Ek__BackingField;
      set
      {
        if (this.\u003CPlantingSiteType\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (PlantingSiteType));
        this.\u003CPlantingSiteType\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.PlantingSiteType);
      }
    }

    public virtual PlotLandUse PlotLandUse
    {
      get => this.\u003CPlotLandUse\u003Ek__BackingField;
      set
      {
        if (this.\u003CPlotLandUse\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (PlotLandUse));
        this.\u003CPlotLandUse\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.PlotLandUse);
      }
    }

    public virtual Street Street
    {
      get => this.\u003CStreet\u003Ek__BackingField;
      set
      {
        if (this.\u003CStreet\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (Street));
        this.\u003CStreet\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Street);
      }
    }

    public virtual string StreetAddress
    {
      get => this.\u003CStreetAddress\u003Ek__BackingField;
      set
      {
        if (string.Equals(this.\u003CStreetAddress\u003Ek__BackingField, value, StringComparison.Ordinal))
          return;
        this.OnPropertyChanging(nameof (StreetAddress));
        this.\u003CStreetAddress\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.StreetAddress);
      }
    }

    public virtual double? xCoordinate
    {
      get => this.\u003CxCoordinate\u003Ek__BackingField;
      set
      {
        if (Nullable.Equals<double>(this.xCoordinate, value))
          return;
        this.OnPropertyChanging(nameof (xCoordinate));
        this.\u003CxCoordinate\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.xCoordinate);
      }
    }

    public virtual double? yCoordinate
    {
      get => this.\u003CyCoordinate\u003Ek__BackingField;
      set
      {
        if (Nullable.Equals<double>(this.yCoordinate, value))
          return;
        this.OnPropertyChanging(nameof (yCoordinate));
        this.\u003CyCoordinate\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.yCoordinate);
      }
    }

    public override object Clone() => (object) PlantingSite.Clone(this, new EntityMap());

    public virtual PlantingSiteDTO GetDTO()
    {
      PlantingSiteDTO dto = new PlantingSiteDTO();
      dto.Guid = this.IsTransient ? new Guid?() : new Guid?(this.Guid);
      dto.PlantingSiteType = this.PlantingSiteType != null ? Util.NullIfDefault<Guid>(this.PlantingSiteType.Guid, new Guid()) : new Guid?();
      dto.PlotLandUse = this.PlotLandUse != null ? Util.NullIfDefault<Guid>(this.PlotLandUse.Guid, new Guid()) : new Guid?();
      dto.Street = this.Street != null ? Util.NullIfDefault<Guid>(this.Street.Guid, new Guid()) : new Guid?();
      dto.StreetAddress = this.StreetAddress;
      dto.xCoordinate = this.xCoordinate;
      dto.yCoordinate = this.yCoordinate;
      dto.Revision = this.Revision;
      return dto;
    }

    public override PlantingSite Clone(bool deep) => PlantingSite.Clone(this, new EntityMap(), deep);

    internal static PlantingSite Clone(PlantingSite ps, EntityMap map, bool deep = true)
    {
      PlantingSite eNew;
      if (map.Contains((Entity) ps))
      {
        eNew = map.GetEntity<PlantingSite>(ps);
      }
      else
      {
        eNew = new PlantingSite();
        eNew.Id = ps.Id;
        eNew.StreetAddress = ps.StreetAddress;
        eNew.xCoordinate = ps.xCoordinate;
        eNew.yCoordinate = ps.yCoordinate;
        map.Add((Entity) ps, (Entity) eNew);
      }
      if (deep)
      {
        eNew.Plot = map.GetEntity<Plot>(ps.Plot);
        eNew.PlantingSiteType = map.GetEntity<PlantingSiteType>(ps.PlantingSiteType);
        eNew.PlotLandUse = map.GetEntity<PlotLandUse>(ps.PlotLandUse);
        eNew.Street = map.GetEntity<Street>(ps.Street);
      }
      return eNew;
    }
  }
}
