// Decompiled with JetBrains decompiler
// Type: Eco.Util.ChildComparer
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Eco.Util
{
  public class ChildComparer : IComparer, IComparer<object>
  {
    private string _child;
    private IComparer _childComparer;

    public ChildComparer(string child, IComparer childComparer)
    {
      if (child == null)
        throw new ArgumentNullException(nameof (child));
      if (childComparer == null)
        throw new ArgumentNullException(nameof (childComparer));
      this._child = child;
      this._childComparer = childComparer;
    }

    public int Compare(object x, object y)
    {
      if (x == null)
        return y != null ? -1 : 0;
      if (y == null)
        return -1;
      PropertyInfo property1 = x.GetType().GetProperty(this._child);
      PropertyInfo property2 = y.GetType().GetProperty(this._child);
      if (property1 == (PropertyInfo) null)
        throw new PropertyNotFoundException(this._child, x.GetType());
      if (property2 == (PropertyInfo) null)
        throw new PropertyNotFoundException(this._child, y.GetType());
      return this._childComparer.Compare(property1.GetValue(x, (object[]) null), property2.GetValue(y, (object[]) null));
    }
  }
}
