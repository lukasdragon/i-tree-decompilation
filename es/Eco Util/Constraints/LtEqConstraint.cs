// Decompiled with JetBrains decompiler
// Type: Eco.Util.Constraints.LtEqConstraint
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

namespace Eco.Util.Constraints
{
  public class LtEqConstraint : OrConstraint
  {
    public LtEqConstraint(object value)
      : base((AConstraint) new LtConstraint(value), (AConstraint) new EqConstraint(value))
    {
    }
  }
}
