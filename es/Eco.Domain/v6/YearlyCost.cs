// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v6.YearlyCost
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using Eco.Domain.DTO;
using Eco.Domain.DTO.v6;
using System;

namespace Eco.Domain.v6
{
  public class YearlyCost : Entity<YearlyCost>
  {
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

    public virtual bool Public
    {
      get => this.\u003CPublic\u003Ek__BackingField;
      set
      {
        if (this.\u003CPublic\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (Public));
        this.\u003CPublic\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Public);
      }
    }

    public virtual Decimal Planting
    {
      get => this.\u003CPlanting\u003Ek__BackingField;
      set
      {
        if (Decimal.Equals(this.\u003CPlanting\u003Ek__BackingField, value))
          return;
        this.OnPropertyChanging(nameof (Planting));
        this.\u003CPlanting\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Planting);
      }
    }

    public virtual Decimal Pruning
    {
      get => this.\u003CPruning\u003Ek__BackingField;
      set
      {
        if (Decimal.Equals(this.\u003CPruning\u003Ek__BackingField, value))
          return;
        this.OnPropertyChanging(nameof (Pruning));
        this.\u003CPruning\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Pruning);
      }
    }

    public virtual Decimal TreeRemoval
    {
      get => this.\u003CTreeRemoval\u003Ek__BackingField;
      set
      {
        if (Decimal.Equals(this.\u003CTreeRemoval\u003Ek__BackingField, value))
          return;
        this.OnPropertyChanging(nameof (TreeRemoval));
        this.\u003CTreeRemoval\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.TreeRemoval);
      }
    }

    public virtual Decimal PestControl
    {
      get => this.\u003CPestControl\u003Ek__BackingField;
      set
      {
        if (Decimal.Equals(this.\u003CPestControl\u003Ek__BackingField, value))
          return;
        this.OnPropertyChanging(nameof (PestControl));
        this.\u003CPestControl\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.PestControl);
      }
    }

    public virtual Decimal Irrigation
    {
      get => this.\u003CIrrigation\u003Ek__BackingField;
      set
      {
        if (Decimal.Equals(this.\u003CIrrigation\u003Ek__BackingField, value))
          return;
        this.OnPropertyChanging(nameof (Irrigation));
        this.\u003CIrrigation\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Irrigation);
      }
    }

    public virtual Decimal Repair
    {
      get => this.\u003CRepair\u003Ek__BackingField;
      set
      {
        if (Decimal.Equals(this.\u003CRepair\u003Ek__BackingField, value))
          return;
        this.OnPropertyChanging(nameof (Repair));
        this.\u003CRepair\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Repair);
      }
    }

    public virtual Decimal CleanUp
    {
      get => this.\u003CCleanUp\u003Ek__BackingField;
      set
      {
        if (Decimal.Equals(this.\u003CCleanUp\u003Ek__BackingField, value))
          return;
        this.OnPropertyChanging(nameof (CleanUp));
        this.\u003CCleanUp\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.CleanUp);
      }
    }

    public virtual Decimal Legal
    {
      get => this.\u003CLegal\u003Ek__BackingField;
      set
      {
        if (Decimal.Equals(this.\u003CLegal\u003Ek__BackingField, value))
          return;
        this.OnPropertyChanging(nameof (Legal));
        this.\u003CLegal\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Legal);
      }
    }

    public virtual Decimal Administrative
    {
      get => this.\u003CAdministrative\u003Ek__BackingField;
      set
      {
        if (Decimal.Equals(this.\u003CAdministrative\u003Ek__BackingField, value))
          return;
        this.OnPropertyChanging(nameof (Administrative));
        this.\u003CAdministrative\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Administrative);
      }
    }

    public virtual Decimal Inspection
    {
      get => this.\u003CInspection\u003Ek__BackingField;
      set
      {
        if (Decimal.Equals(this.\u003CInspection\u003Ek__BackingField, value))
          return;
        this.OnPropertyChanging(nameof (Inspection));
        this.\u003CInspection\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Inspection);
      }
    }

    public virtual Decimal Other
    {
      get => this.\u003COther\u003Ek__BackingField;
      set
      {
        if (Decimal.Equals(this.\u003COther\u003Ek__BackingField, value))
          return;
        this.OnPropertyChanging(nameof (Other));
        this.\u003COther\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Other);
      }
    }

    public virtual YearlyCostDTO GetDTO()
    {
      YearlyCostDTO dto = new YearlyCostDTO();
      dto.Guid = this.IsTransient ? new Guid?() : new Guid?(this.Guid);
      dto.Public = this.Public;
      dto.Planting = Util.NullIfDefault<Decimal>(this.Planting, 0M);
      dto.Pruning = Util.NullIfDefault<Decimal>(this.Pruning, 0M);
      dto.TreeRemoval = Util.NullIfDefault<Decimal>(this.TreeRemoval, 0M);
      dto.PestControl = Util.NullIfDefault<Decimal>(this.PestControl, 0M);
      dto.Irrigation = Util.NullIfDefault<Decimal>(this.Irrigation, 0M);
      dto.Repair = Util.NullIfDefault<Decimal>(this.Repair, 0M);
      dto.CleanUp = Util.NullIfDefault<Decimal>(this.CleanUp, 0M);
      dto.Legal = Util.NullIfDefault<Decimal>(this.Legal, 0M);
      dto.Administrative = Util.NullIfDefault<Decimal>(this.Administrative, 0M);
      dto.Inspection = Util.NullIfDefault<Decimal>(this.Inspection, 0M);
      dto.Other = Util.NullIfDefault<Decimal>(this.Other, 0M);
      dto.Revision = this.Revision;
      return dto;
    }

    public override YearlyCost Clone(bool deep) => YearlyCost.Clone(this, new EntityMap(), deep);

    public override object Clone() => (object) YearlyCost.Clone(this, new EntityMap());

    internal static YearlyCost Clone(YearlyCost yc, EntityMap map, bool deep = true)
    {
      YearlyCost eNew;
      if (map.Contains((Entity) yc))
      {
        eNew = map.GetEntity<YearlyCost>(yc);
      }
      else
      {
        eNew = new YearlyCost();
        eNew.Public = yc.Public;
        eNew.Planting = yc.Planting;
        eNew.Pruning = yc.Pruning;
        eNew.TreeRemoval = yc.TreeRemoval;
        eNew.PestControl = yc.PestControl;
        eNew.Irrigation = yc.Irrigation;
        eNew.Repair = yc.Repair;
        eNew.CleanUp = yc.CleanUp;
        eNew.Legal = yc.Legal;
        eNew.Administrative = yc.Administrative;
        eNew.Inspection = yc.Inspection;
        eNew.Other = yc.Other;
        map.Add((Entity) yc, (Entity) eNew);
      }
      eNew.Year = map.GetEntity<Year>(yc.Year);
      return eNew;
    }
  }
}
