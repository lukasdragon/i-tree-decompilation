// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v6.DBHRptClass
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

namespace Eco.Domain.v6
{
  public class DBHRptClass : Entity<DBHRptClass>
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

    public virtual double RangeStart
    {
      get => this.\u003CRangeStart\u003Ek__BackingField;
      set
      {
        if (this.\u003CRangeStart\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (RangeStart));
        this.\u003CRangeStart\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RangeStart);
      }
    }

    public virtual double RangeEnd
    {
      get => this.\u003CRangeEnd\u003Ek__BackingField;
      set
      {
        if (this.\u003CRangeEnd\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (RangeEnd));
        this.\u003CRangeEnd\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RangeEnd);
      }
    }

    public override DBHRptClass Clone(bool deep) => DBHRptClass.Clone(this, new EntityMap(), deep);

    public override object Clone() => (object) DBHRptClass.Clone(this, new EntityMap());

    internal static DBHRptClass Clone(DBHRptClass c, EntityMap map, bool deep = true)
    {
      DBHRptClass eNew;
      if (map.Contains((Entity) c))
      {
        eNew = map.GetEntity<DBHRptClass>(c);
      }
      else
      {
        eNew = new DBHRptClass();
        eNew.Id = c.Id;
        eNew.RangeStart = c.RangeStart;
        eNew.RangeEnd = c.RangeEnd;
        map.Add((Entity) c, (Entity) eNew);
      }
      if (deep)
        eNew.Year = map.GetEntity<Year>(c.Year);
      return eNew;
    }
  }
}
