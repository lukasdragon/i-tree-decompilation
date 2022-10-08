// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v6.PlotLandUse
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using Eco.Domain.DTO.v6;
using System;

namespace Eco.Domain.v6
{
  public class PlotLandUse : Entity<PlotLandUse>
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

    public virtual LandUse LandUse
    {
      get => this.\u003CLandUse\u003Ek__BackingField;
      set
      {
        if (this.\u003CLandUse\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (LandUse));
        this.\u003CLandUse\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.LandUse);
      }
    }

    public virtual short PercentOfPlot
    {
      get => this.\u003CPercentOfPlot\u003Ek__BackingField;
      set
      {
        if ((int) this.\u003CPercentOfPlot\u003Ek__BackingField == (int) value)
          return;
        this.OnPropertyChanging(nameof (PercentOfPlot));
        this.\u003CPercentOfPlot\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.PercentOfPlot);
      }
    }

    public virtual PlotLandUseDTO GetDTO()
    {
      PlotLandUseDTO dto = new PlotLandUseDTO();
      dto.Guid = this.IsTransient ? new Guid?() : new Guid?(this.Guid);
      dto.LandUse = this.LandUse?.Guid;
      dto.PercentOfPlot = this.PercentOfPlot;
      dto.Revision = this.Revision;
      return dto;
    }

    public override PlotLandUse Clone(bool deep) => PlotLandUse.Clone(this, new EntityMap(), deep);

    public override object Clone() => (object) PlotLandUse.Clone(this, new EntityMap());

    internal static PlotLandUse Clone(PlotLandUse pflu, EntityMap map, bool deep = true)
    {
      PlotLandUse eNew;
      if (map.Contains((Entity) pflu))
      {
        eNew = map.GetEntity<PlotLandUse>(pflu);
      }
      else
      {
        eNew = new PlotLandUse();
        eNew.PercentOfPlot = pflu.PercentOfPlot;
        map.Add((Entity) pflu, (Entity) eNew);
      }
      if (deep)
      {
        eNew.Plot = map.GetEntity<Plot>(pflu.Plot);
        eNew.LandUse = map.GetEntity<LandUse>(pflu.LandUse);
      }
      return eNew;
    }
  }
}
