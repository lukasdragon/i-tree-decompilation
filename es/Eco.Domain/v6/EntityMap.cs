// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v6.EntityMap
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using System.Collections.Generic;

namespace Eco.Domain.v6
{
  public class EntityMap
  {
    private Dictionary<Entity, Entity> m_map;

    public EntityMap() => this.m_map = new Dictionary<Entity, Entity>();

    public virtual T GetEntity<T>(T ent_in) where T : Entity
    {
      if ((object) ent_in != null)
      {
        Entity entity = (Entity) null;
        if (this.m_map.TryGetValue((Entity) ent_in, out entity))
          return entity as T;
      }
      return ent_in;
    }

    public virtual void Add(Entity eOld, Entity eNew)
    {
      if (eOld == null || eNew == null)
        return;
      this.m_map.Add(eOld, eNew);
    }

    public virtual bool Contains(Entity e) => e != null && this.m_map.ContainsKey(e);
  }
}
