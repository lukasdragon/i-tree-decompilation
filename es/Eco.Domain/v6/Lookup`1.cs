// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v6.Lookup`1
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using Eco.Domain.DTO.v6;
using System;

namespace Eco.Domain.v6
{
  public abstract class Lookup<T> : Lookup where T : Lookup<T>
  {
    public virtual LookupDTO GetDTO()
    {
      LookupDTO dto = new LookupDTO();
      dto.Guid = this.IsTransient ? new Guid?() : new Guid?(this.Guid);
      dto.Id = this.Id;
      dto.Description = this.Description;
      return dto;
    }

    public override Lookup Clone(bool deep) => (Lookup) Lookup<T>.Clone((T) this, new EntityMap(), deep);

    public override object Clone() => (object) Lookup<T>.Clone((T) this, new EntityMap());

    internal static T Clone(T lu, EntityMap map, bool deep = true)
    {
      T eNew = default (T);
      if ((object) lu != null)
      {
        if (map.Contains((Entity) lu))
        {
          eNew = map.GetEntity<T>(lu);
        }
        else
        {
          eNew = Activator.CreateInstance<T>();
          eNew.Id = lu.Id;
          eNew.Description = lu.Description;
          map.Add((Entity) lu, (Entity) eNew);
        }
        if (deep)
          eNew.Year = map.GetEntity<Year>(lu.Year);
      }
      return eNew;
    }
  }
}
