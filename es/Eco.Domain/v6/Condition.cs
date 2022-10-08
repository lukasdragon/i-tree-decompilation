// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v6.Condition
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using Eco.Domain.Attributes;
using Eco.Domain.DTO.v6;
using Eco.Domain.Properties;
using System;

namespace Eco.Domain.v6
{
  public class Condition : Entity<Condition>
  {
    private string m_description;
    private string m_diebackDesc;

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

    [LocalizedDescription(typeof (v6Strings), "Condition_PctDieback")]
    public virtual double PctDieback
    {
      get => this.\u003CPctDieback\u003Ek__BackingField;
      set
      {
        if (this.\u003CPctDieback\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging("Description");
        this.OnPropertyChanging("DiebackDesc");
        this.OnPropertyChanging("Percent");
        this.OnPropertyChanging("Category");
        this.OnPropertyChanging(nameof (PctDieback));
        this.\u003CPctDieback\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Description);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.DiebackDesc);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Percent);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Category);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.PctDieback);
      }
    }

    [LocalizedDescription(typeof (v6Strings), "Condition_Description")]
    public virtual string Description
    {
      get => this.m_description == null ? string.Format("{0}%", (object) (100.0 - this.PctDieback)) : this.m_description;
      set
      {
        if (string.Equals(this.m_description, value, StringComparison.Ordinal))
          return;
        this.OnPropertyChanging(nameof (Description));
        this.m_description = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Description);
      }
    }

    [LocalizedDescription(typeof (v6Strings), "Condition_Description")]
    public virtual string DiebackDesc
    {
      get => this.m_diebackDesc == null ? string.Format("{0}%", (object) this.PctDieback) : this.m_diebackDesc;
      set
      {
        if (string.Equals(this.m_diebackDesc, value, StringComparison.Ordinal))
          return;
        this.OnPropertyChanging(nameof (DiebackDesc));
        this.m_diebackDesc = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.DiebackDesc);
      }
    }

    public virtual ConditionDTO GetDTO()
    {
      ConditionDTO dto = new ConditionDTO();
      dto.Guid = this.IsTransient ? new Guid?() : new Guid?(this.Guid);
      dto.Id = this.Id;
      dto.Description = this.Description;
      dto.PctDieback = this.PctDieback;
      dto.DiebackDesc = this.DiebackDesc;
      dto.Revision = this.Revision;
      return dto;
    }

    [LocalizedDescription(typeof (v6Strings), "Condition_Percent")]
    public virtual double Percent
    {
      get => 100.0 - this.PctDieback;
      set
      {
        if (this.Percent == value)
          return;
        this.OnPropertyChanging(nameof (Percent));
        this.PctDieback = 100.0 - value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Percent);
      }
    }

    public override Condition Clone(bool deep) => Condition.Clone(this, new EntityMap(), deep);

    public override object Clone() => (object) Condition.Clone(this, new EntityMap());

    public static Condition Default => new Condition()
    {
      PctDieback = 13.0
    };

    public virtual ConditionCategory Category
    {
      get
      {
        double pctDieback = this.PctDieback;
        foreach (ConditionCategory category in Enum.GetValues(typeof (ConditionCategory)))
        {
          if (pctDieback <= (double) category)
            return category;
        }
        return ConditionCategory.Dead;
      }
    }

    internal static Condition Clone(Condition c, EntityMap map, bool deep = true)
    {
      Condition eNew;
      if (map.Contains((Entity) c))
      {
        eNew = map.GetEntity<Condition>(c);
      }
      else
      {
        eNew = new Condition();
        eNew.Id = c.Id;
        eNew.Description = c.Description;
        eNew.PctDieback = c.PctDieback;
        eNew.DiebackDesc = c.DiebackDesc;
        map.Add((Entity) c, (Entity) eNew);
      }
      if (deep)
        eNew.Year = map.GetEntity<Year>(c.Year);
      return eNew;
    }
  }
}
