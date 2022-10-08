// Decompiled with JetBrains decompiler
// Type: Eco.Util.ObjectComparer
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using System;
using System.Collections;
using System.Collections.Generic;

namespace Eco.Util
{
  public class ObjectComparer : IComparer, IComparer<object>
  {
    public int Compare(object x, object y)
    {
      if (x == y)
        return 0;
      if (x == null)
        return -1;
      if (y == null)
        return 1;
      if (x.GetType() == y.GetType())
      {
        if (x is IComparable comparable1)
          return comparable1.CompareTo(y);
        if (y is IComparable comparable2)
          return -comparable2.CompareTo(x);
      }
      return x.Equals(y) ? 0 : x.ToString().CompareTo(y.ToString());
    }
  }
}
