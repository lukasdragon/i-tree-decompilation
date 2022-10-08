// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v6.LandUse
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using Eco.Domain.Attributes;
using Eco.Domain.DTO;
using Eco.Domain.DTO.v6;
using Eco.Domain.Properties;
using System;
using System.Collections.Generic;

namespace Eco.Domain.v6
{
  public class LandUse : Entity<LandUse>
  {
    public LandUse()
    {
      this.LandUseId = 0;
      this.Init();
    }

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

    public virtual ISet<PlotLandUse> PlotLandUses
    {
      get => this.\u003CPlotLandUses\u003Ek__BackingField;
      protected internal set
      {
        if (this.PlotLandUses == value)
          return;
        this.OnPropertyChanging(nameof (PlotLandUses));
        this.\u003CPlotLandUses\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.PlotLandUses);
      }
    }

    [LocalizedDescription(typeof (v6Strings), "LandUse_Id")]
    public virtual char Id
    {
      get => this.\u003CId\u003Ek__BackingField;
      set
      {
        if ((int) this.\u003CId\u003Ek__BackingField == (int) value)
          return;
        this.OnPropertyChanging(nameof (Id));
        this.\u003CId\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Id);
      }
    }

    [LocalizedDescription(typeof (v6Strings), "LandUse_Description")]
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

    [LocalizedDescription(typeof (v6Strings), "LandUse_LandUseId")]
    public virtual int LandUseId
    {
      get => this.\u003CLandUseId\u003Ek__BackingField;
      set
      {
        if (this.\u003CLandUseId\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (LandUseId));
        this.\u003CLandUseId\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.LandUseId);
      }
    }

    private void Init() => this.PlotLandUses = (ISet<PlotLandUse>) new HashSet<PlotLandUse>();

    public virtual LandUseDTO GetDTO()
    {
      LandUseDTO dto = new LandUseDTO();
      dto.Guid = this.IsTransient ? new Guid?() : new Guid?(this.Guid);
      dto.Id = this.Id;
      dto.Description = this.Description;
      dto.LandUseId = Util.NullIfDefault<int>(this.LandUseId, 0);
      dto.Revision = this.Revision;
      return dto;
    }

    public override LandUse Clone(bool deep) => LandUse.Clone(this, new EntityMap(), deep);

    public override object Clone() => (object) LandUse.Clone(this, new EntityMap());

    internal static LandUse Clone(LandUse flu, EntityMap map, bool deep = true)
    {
      LandUse eNew;
      if (map.Contains((Entity) flu))
      {
        eNew = map.GetEntity<LandUse>(flu);
      }
      else
      {
        eNew = new LandUse();
        eNew.Id = flu.Id;
        eNew.Description = flu.Description;
        eNew.LandUseId = flu.LandUseId;
        map.Add((Entity) flu, (Entity) eNew);
      }
      if (deep)
        eNew.Year = map.GetEntity<Year>(flu.Year);
      return eNew;
    }
  }
}
