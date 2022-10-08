// Decompiled with JetBrains decompiler
// Type: Eco.Util.Constraints.EqConstraint
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using System;

namespace Eco.Util.Constraints
{
  public class EqConstraint : AConstraint
  {
    private object _x;

    public EqConstraint(object lhs) => this._x = lhs;

    public override bool Test(object y)
    {
      object x = this._x;
      if (x == y)
        return true;
      if (x == null || y == null)
        return false;
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
        return comparable1.CompareTo(y) == 0;
      return y is IComparable comparable2 ? comparable2.CompareTo(x) == 0 : x.Equals(y);
    }
  }
}
