// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v6.PlotGroundCover
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using Eco.Domain.DTO.v6;
using System;

namespace Eco.Domain.v6
{
  public class PlotGroundCover : Entity<PlotGroundCover>
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

    public virtual GroundCover GroundCover
    {
      get => this.\u003CGroundCover\u003Ek__BackingField;
      set
      {
        if (this.\u003CGroundCover\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (GroundCover));
        this.\u003CGroundCover\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.GroundCover);
      }
    }

    public virtual int PercentCovered
    {
      get => this.\u003CPercentCovered\u003Ek__BackingField;
      set
      {
        if (this.\u003CPercentCovered\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (PercentCovered));
        this.\u003CPercentCovered\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.PercentCovered);
      }
    }

    public virtual PlotGroundCoverDTO GetDTO()
    {
      PlotGroundCoverDTO dto = new PlotGroundCoverDTO();
      dto.Guid = this.IsTransient ? new Guid?() : new Guid?(this.Guid);
      dto.GroundCover = this.GroundCover?.Guid;
      dto.PercentCovered = this.PercentCovered;
      dto.Revision = this.Revision;
      return dto;
    }

    public override PlotGroundCover Clone(bool deep) => PlotGroundCover.Clone(this, new EntityMap(), deep);

    public override object Clone() => (object) PlotGroundCover.Clone(this, new EntityMap());

    internal static PlotGroundCover Clone(
      PlotGroundCover pgc,
      EntityMap map,
      bool deep = true)
    {
      PlotGroundCover eNew;
      if (map.Contains((Entity) pgc))
      {
        eNew = map.GetEntity<PlotGroundCover>(pgc);
      }
      else
      {
        eNew = new PlotGroundCover();
        eNew.PercentCovered = pgc.PercentCovered;
        map.Add((Entity) pgc, (Entity) eNew);
      }
      if (deep)
      {
        eNew.Plot = map.GetEntity<Plot>(pgc.Plot);
        eNew.GroundCover = map.GetEntity<GroundCover>(pgc.GroundCover);
      }
      return eNew;
    }
  }
}
