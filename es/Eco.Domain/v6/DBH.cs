// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v6.DBH
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using Eco.Domain.DTO.v6;
using System;

namespace Eco.Domain.v6
{
  public class DBH : Entity<DBH>
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
        this.OnPropertyChanging("DBHId");
        this.OnPropertyChanging(nameof (Id));
        this.\u003CId\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.DBHId);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Id);
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

    public virtual double Value
    {
      get => this.\u003CValue\u003Ek__BackingField;
      set
      {
        if (this.\u003CValue\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (Value));
        this.\u003CValue\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Value);
      }
    }

    public virtual DBHDTO GetDTO()
    {
      DBHDTO dto = new DBHDTO();
      dto.Guid = this.IsTransient ? new Guid?() : new Guid?(this.Guid);
      dto.Id = this.Id;
      dto.Description = this.Description;
      dto.Value = this.Value;
      return dto;
    }

    public virtual double DBHId => (double) this.Id;

    public override DBH Clone(bool deep) => DBH.Clone(this, new EntityMap(), deep);

    public override object Clone() => (object) DBH.Clone(this, new EntityMap());

    internal static DBH Clone(DBH dbh, EntityMap map, bool deep = true)
    {
      DBH eNew;
      if (map.Contains((Entity) dbh))
      {
        eNew = map.GetEntity<DBH>(dbh);
      }
      else
      {
        eNew = new DBH();
        eNew.Id = dbh.Id;
        eNew.Description = dbh.Description;
        eNew.Value = dbh.Value;
        map.Add((Entity) dbh, (Entity) eNew);
      }
      if (deep)
        eNew.Year = map.GetEntity<Year>(dbh.Year);
      return eNew;
    }
  }
}
