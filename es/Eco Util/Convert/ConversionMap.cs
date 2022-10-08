// Decompiled with JetBrains decompiler
// Type: Eco.Util.Convert.ConversionMap
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using Eco.Domain.v6;
using System.Collections.Generic;

namespace Eco.Util.Convert
{
  public class ConversionMap
  {
    private Dictionary<object, Entity> m_map;

    public ConversionMap() => this.m_map = new Dictionary<object, Entity>();

    public T GetEntity<T>(object key) where T : Entity
    {
      if (key != null)
      {
        Entity entity = (Entity) null;
        if (this.m_map.TryGetValue(key, out entity))
          return entity as T;
      }
      return default (T);
    }

    public void Add(object key, Entity value)
    {
      if (key == null || value == null)
        return;
      this.m_map.Add(key, value);
    }

    public bool Contains(object key) => key != null && this.m_map.ContainsKey(key);
  }
}
