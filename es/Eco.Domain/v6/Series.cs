// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v6.Series
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using Eco.Domain.DTO;
using Eco.Domain.DTO.v6;
using System;
using System.Collections.Generic;

namespace Eco.Domain.v6
{
  public class Series : Entity<Series>
  {
    public Series()
    {
      this.Init();
      this.DefaultPlotSize = 0.04f;
      this.DefaultPlotSizeUnit = PlotSize.Acres;
      this.SampleMethod = SampleMethod.None;
      this.SampleType = SampleType.RegularPlot;
    }

    public virtual Project Project
    {
      get => this.\u003CProject\u003Ek__BackingField;
      set
      {
        if (this.\u003CProject\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (Project));
        this.\u003CProject\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Project);
      }
    }

    public virtual ISet<Year> Years
    {
      get => this.\u003CYears\u003Ek__BackingField;
      protected internal set
      {
        if (this.Years == value)
          return;
        this.OnPropertyChanging(nameof (Years));
        this.\u003CYears\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Years);
      }
    }

    public virtual string Id
    {
      get => this.\u003CId\u003Ek__BackingField;
      set
      {
        if (string.Equals(this.\u003CId\u003Ek__BackingField, value, StringComparison.Ordinal))
          return;
        this.OnPropertyChanging(nameof (Id));
        this.\u003CId\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Id);
      }
    }

    public virtual bool IsPermanent
    {
      get => this.\u003CIsPermanent\u003Ek__BackingField;
      set
      {
        if (this.\u003CIsPermanent\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (IsPermanent));
        this.\u003CIsPermanent\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.IsPermanent);
      }
    }

    public virtual SampleType SampleType
    {
      get => this.\u003CSampleType\u003Ek__BackingField;
      set
      {
        if (this.\u003CSampleType\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging("IsSample");
        this.OnPropertyChanging(nameof (SampleType));
        this.\u003CSampleType\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.IsSample);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.SampleType);
      }
    }

    public virtual SampleMethod SampleMethod
    {
      get => this.\u003CSampleMethod\u003Ek__BackingField;
      set
      {
        if (this.\u003CSampleMethod\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (SampleMethod));
        this.\u003CSampleMethod\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.SampleMethod);
      }
    }

    public virtual float DefaultPlotSize
    {
      get => this.\u003CDefaultPlotSize\u003Ek__BackingField;
      set
      {
        if (this.\u003CDefaultPlotSize\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (DefaultPlotSize));
        this.\u003CDefaultPlotSize\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.DefaultPlotSize);
      }
    }

    public virtual PlotSize DefaultPlotSizeUnit
    {
      get => this.\u003CDefaultPlotSizeUnit\u003Ek__BackingField;
      set
      {
        if (this.\u003CDefaultPlotSizeUnit\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (DefaultPlotSizeUnit));
        this.\u003CDefaultPlotSizeUnit\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.DefaultPlotSizeUnit);
      }
    }

    public virtual string GISProjection
    {
      get => this.\u003CGISProjection\u003Ek__BackingField;
      set
      {
        if (string.Equals(this.\u003CGISProjection\u003Ek__BackingField, value, StringComparison.Ordinal))
          return;
        this.OnPropertyChanging(nameof (GISProjection));
        this.\u003CGISProjection\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.GISProjection);
      }
    }

    public virtual short GISUnit
    {
      get => this.\u003CGISUnit\u003Ek__BackingField;
      set
      {
        if ((int) this.\u003CGISUnit\u003Ek__BackingField == (int) value)
          return;
        this.OnPropertyChanging(nameof (GISUnit));
        this.\u003CGISUnit\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.GISUnit);
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

    private void Init() => this.Years = (ISet<Year>) new HashSet<Year>();

    public virtual SeriesDTO GetDTO()
    {
      SeriesDTO dto = new SeriesDTO();
      dto.Guid = this.IsTransient ? new Guid?() : new Guid?(this.Guid);
      dto.Id = this.Id;
      dto.Comments = this.Comments;
      dto.DefaultPlotSize = Util.NullIfDefault(this.DefaultPlotSize, -1f);
      dto.DefaultPlotSizeUnit = (char) this.DefaultPlotSizeUnit;
      dto.IsPermanent = Util.NullIfDefault<bool>(this.IsPermanent, true);
      dto.SampleMethod = Util.NullIfDefault<int>((int) this.SampleMethod, 0);
      dto.SampleType = (char) this.SampleType;
      dto.Project = this.Project.GetDTO();
      dto.Revision = this.Revision;
      return dto;
    }

    public virtual bool IsSample => this.SampleType != SampleType.Inventory;

    public override Series Clone(bool deep) => Series.Clone(this, new EntityMap(), false);

    public override object Clone() => (object) Series.Clone(this, new EntityMap());

    internal static Series Clone(Series s, EntityMap map, bool deep = true)
    {
      Series eNew;
      if (map.Contains((Entity) s))
      {
        eNew = map.GetEntity<Series>(s);
      }
      else
      {
        eNew = new Series();
        eNew.Id = s.Id;
        eNew.IsPermanent = s.IsPermanent;
        eNew.SampleType = s.SampleType;
        eNew.SampleMethod = s.SampleMethod;
        eNew.DefaultPlotSize = s.DefaultPlotSize;
        eNew.DefaultPlotSizeUnit = s.DefaultPlotSizeUnit;
        eNew.Comments = s.Comments;
        map.Add((Entity) s, (Entity) eNew);
        if (deep)
        {
          foreach (Year year in (IEnumerable<Year>) s.Years)
            eNew.Years.Add(Year.Clone(year, map));
        }
      }
      if (deep)
        eNew.Project = map.GetEntity<Project>(s.Project);
      return eNew;
    }
  }
}
