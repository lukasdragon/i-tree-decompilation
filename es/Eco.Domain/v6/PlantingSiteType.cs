// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v6.PlantingSiteType
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using Eco.Domain.DTO.v6;
using System;

namespace Eco.Domain.v6
{
  public class PlantingSiteType : Entity<PlantingSiteType>
  {
    public PlantingSiteType() => this.Size = -1.0;

    public virtual Year Year
    {
      get => this.\u003CYear\u003Ek__BackingField;
      set
      {
        if (this.\u003CYear\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (Year));
        this.\u003CYear\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Year);
      }
    }

    public virtual string Description
    {
      get => this.\u003CDescription\u003Ek__BackingField;
      set
      {
        if (string.Equals(this.\u003CDescription\u003Ek__BackingField, value, StringComparison.Ordinal))
          return;
        this.OnPropertyChanging(nameof (Description));
        this.\u003CDescription\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Description);
      }
    }

    public virtual double Size
    {
      get => this.\u003CSize\u003Ek__BackingField;
      set
      {
        if (this.\u003CSize\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (Size));
        this.\u003CSize\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Size);
      }
    }

    public virtual PlantingSiteTypeDTO GetDTO()
    {
      PlantingSiteTypeDTO dto = new PlantingSiteTypeDTO();
      dto.Guid = this.IsTransient ? new Guid?() : new Guid?(this.Guid);
      dto.Description = this.Description;
      dto.Size = this.Size;
      dto.Revision = this.Revision;
      return dto;
    }

    public override PlantingSiteType Clone(bool deep) => PlantingSiteType.Clone(this, new EntityMap(), deep);

    public override object Clone() => (object) PlantingSiteType.Clone(this, new EntityMap());

    internal static PlantingSiteType Clone(
      PlantingSiteType pst,
      EntityMap map,
      bool deep = true)
    {
      PlantingSiteType plantingSiteType;
      if (map.Contains((Entity) pst))
      {
        plantingSiteType = map.GetEntity<PlantingSiteType>(pst);
      }
      else
      {
        plantingSiteType = new PlantingSiteType();
        plantingSiteType.Description = pst.Description;
        plantingSiteType.Size = pst.Size;
      }
      if (deep)
        plantingSiteType.Year = map.GetEntity<Year>(pst.Year);
      return plantingSiteType;
    }
  }
}
