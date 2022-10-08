// Decompiled with JetBrains decompiler
// Type: Eco.Util.Constraints.LtConstraint
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using System;

namespace Eco.Util.Constraints
{
  public class LtConstraint : AConstraint
  {
    private object _x;

    public LtConstraint(object value) => this._x = value;

    public override bool Test(object y)
    {
      object x = this._x;
      if (x == y || x == null)
        return false;
      if (y == null)
        return true;
      Type type1 = y.GetType();
      Type type2 = x.GetType();
      if (type2 != type1)
      {
        try
        {
          y = Convert.ChangeType(y, type2);
          y.GetType();
        }
        catch
        {
          return false;
        }
      }
      if (x is IComparable comparable1)
        return comparable1.CompareTo(y) == 1;
      return y is IComparable comparable2 && comparable2.CompareTo(x) == 1;
    }
  }
}
