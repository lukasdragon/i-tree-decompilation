// Decompiled with JetBrains decompiler
// Type: Eco.Util.Constraints.AndConstraint
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

namespace Eco.Util.Constraints
{
  public class AndConstraint : AConstraint
  {
    private AConstraint _lhs;
    private AConstraint _rhs;

    public AndConstraint(AConstraint lhs, AConstraint rhs)
    {
      this._lhs = lhs;
      this._rhs = rhs;
    }

    public override bool Test(object value) => this._lhs.Test(value) && this._rhs.Test(value);
  }
}
