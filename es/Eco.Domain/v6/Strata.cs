// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v6.Strata
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
  public class Strata : Entity<Strata>
  {
    public Strata()
    {
      this.Init();
      this.Size = -1f;
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

    [LocalizedDescription(typeof (v6Strings), "Entity_Id")]
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

    public virtual ISet<Plot> Plots
    {
      get => this.\u003CPlots\u003Ek__BackingField;
      protected internal set
      {
        if (this.Plots == value)
          return;
        this.OnPropertyChanging(nameof (Plots));
        this.\u003CPlots\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Plots);
      }
    }

    [LocalizedDescription(typeof (v6Strings), "Strata_Description")]
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

    [LocalizedDescription(typeof (v6Strings), "Strata_Abbreviation")]
    public virtual string Abbreviation
    {
      get => this.\u003CAbbreviation\u003Ek__BackingField;
      set
      {
        if (string.Equals(this.\u003CAbbreviation\u003Ek__BackingField, value, StringComparison.Ordinal))
          return;
        this.OnPropertyChanging(nameof (Abbreviation));
        this.\u003CAbbreviation\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Abbreviation);
      }
    }

    [LocalizedDescription(typeof (v6Strings), "Strata_Size")]
    public virtual float Size
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

    private void Init() => this.Plots = (ISet<Plot>) new HashSet<Plot>();

    public virtual StrataDTO GetDTO()
    {
      StrataDTO dto = new StrataDTO();
      dto.Guid = this.IsTransient ? new Guid?() : new Guid?(this.Guid);
      dto.Id = this.Id;
      dto.Abbreviation = this.Abbreviation;
      dto.Description = this.Description;
      dto.Size = Util.NullIfDefault(this.Size, -1f);
      dto.Revision = this.Revision;
      return dto;
    }

    public override Strata Clone(bool deep) => Strata.Clone(this, new EntityMap(), deep);

    public override object Clone() => (object) Strata.Clone(this, new EntityMap());

    internal static Strata Clone(Strata s, EntityMap map, bool deep = true)
    {
      Strata eNew;
      if (map.Contains((Entity) s))
      {
        eNew = map.GetEntity<Strata>(s);
      }
      else
      {
        eNew = new Strata();
        eNew.Id = s.Id;
        eNew.Abbreviation = s.Abbreviation;
        eNew.Description = s.Description;
        eNew.Size = s.Size;
        map.Add((Entity) s, (Entity) eNew);
        if (deep)
        {
          foreach (Plot plot in (IEnumerable<Plot>) s.Plots)
            eNew.Plots.Add(Plot.Clone(plot, map));
        }
      }
      if (deep)
        eNew.Year = map.GetEntity<Year>(s.Year);
      return eNew;
    }
  }
}
