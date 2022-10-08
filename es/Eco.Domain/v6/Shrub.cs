// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v6.Shrub
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using Eco.Domain.DTO.v6;
using System;

namespace Eco.Domain.v6
{
  public class Shrub : Entity<Shrub>
  {
    public Shrub()
    {
      this.PercentOfShrubArea = -1;
      this.Height = -1f;
      this.PercentMissing = PctMidRange.PRINV;
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

    public virtual int PercentOfShrubArea
    {
      get => this.\u003CPercentOfShrubArea\u003Ek__BackingField;
      set
      {
        if (this.\u003CPercentOfShrubArea\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (PercentOfShrubArea));
        this.\u003CPercentOfShrubArea\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.PercentOfShrubArea);
      }
    }

    public virtual string Species
    {
      get => this.\u003CSpecies\u003Ek__BackingField;
      set
      {
        if (string.Equals(this.\u003CSpecies\u003Ek__BackingField, value, StringComparison.Ordinal))
          return;
        this.OnPropertyChanging(nameof (Species));
        this.\u003CSpecies\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Species);
      }
    }

    public virtual float Height
    {
      get => this.\u003CHeight\u003Ek__BackingField;
      set
      {
        if (this.\u003CHeight\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (Height));
        this.\u003CHeight\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Height);
      }
    }

    public virtual PctMidRange PercentMissing
    {
      get => this.\u003CPercentMissing\u003Ek__BackingField;
      set
      {
        if (this.\u003CPercentMissing\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (PercentMissing));
        this.\u003CPercentMissing\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.PercentMissing);
      }
    }

    public virtual string Comments
    {
      get => this.\u003CComments\u003Ek__BackingField;
      set
      {
        if (string.Equals(this.\u003CComments\u003Ek__BackingField, value, StringComparison.Ordinal))
          return;
        this.OnPropertyChanging(nameof (Comments));
        this.\u003CComments\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Comments);
      }
    }

    public virtual ShrubDTO GetDTO()
    {
      ShrubDTO dto = new ShrubDTO();
      dto.Guid = this.IsTransient ? new Guid?() : new Guid?(this.Guid);
      dto.Id = this.Id;
      dto.Comments = this.Comments;
      dto.Height = new float?(this.Height);
      dto.PercentMissing = new int?((int) this.PercentMissing);
      dto.PercentOfShrubArea = new int?(this.PercentOfShrubArea);
      dto.Species = this.Species;
      dto.Revision = this.Revision;
      return dto;
    }

    public override Shrub Clone(bool deep) => Shrub.Clone(this, new EntityMap(), deep);

    public override object Clone() => (object) Shrub.Clone(this, new EntityMap());

    internal static Shrub Clone(Shrub s, EntityMap map, bool deep = true)
    {
      Shrub eNew;
      if (map.Contains((Entity) s))
      {
        eNew = map.GetEntity<Shrub>(s);
      }
      else
      {
        eNew = new Shrub();
        eNew.Id = s.Id;
        eNew.Comments = s.Comments;
        eNew.Height = s.Height;
        eNew.PercentMissing = s.PercentMissing;
        eNew.PercentOfShrubArea = s.PercentOfShrubArea;
        eNew.Species = s.Species;
        map.Add((Entity) s, (Entity) eNew);
      }
      if (deep)
        eNew.Plot = map.GetEntity<Plot>(s.Plot);
      return eNew;
    }
  }
}
