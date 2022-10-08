// Decompiled with JetBrains decompiler
// Type: Eco.Util.PropertyComparer
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using DaveyTree.Core;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Eco.Util
{
  public class PropertyComparer : IComparer, IComparer<object>
  {
    private Dictionary<Type, Func<object, object>> _getPropertyCache;
    private string _property;
    private ObjectComparer _oc;

    public PropertyComparer(string property)
    {
      this._getPropertyCache = new Dictionary<Type, Func<object, object>>();
      this._property = property;
      this._oc = new ObjectComparer();
    }

    public int Compare(object x, object y)
    {
      if (x == null)
        return y != null ? -1 : 0;
      if (y == null)
        return 1;
      Type type1 = x.GetType();
      Type type2 = y.GetType();
      Func<object, object> propertyGetter1;
      if (!this._getPropertyCache.TryGetValue(type1, out propertyGetter1))
      {
        propertyGetter1 = TypeHelper.GetPropertyGetter(type1, this._property);
        this._getPropertyCache.Add(type1, propertyGetter1);
      }
      Func<object, object> propertyGetter2;
      if (!this._getPropertyCache.TryGetValue(type2, out propertyGetter2))
      {
        propertyGetter2 = TypeHelper.GetPropertyGetter(type2, this._property);
        this._getPropertyCache.Add(type2, propertyGetter2);
      }
      if (propertyGetter1 == null)
        throw new PropertyNotFoundException(this._property, type1);
      if (propertyGetter2 == null)
        throw new PropertyNotFoundException(this._property, type2);
      return this._oc.Compare(propertyGetter1(x), propertyGetter2(y));
    }
  }
}
