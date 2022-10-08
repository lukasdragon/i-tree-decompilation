// Decompiled with JetBrains decompiler
// Type: Eco.Util.FieldConstraint
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using Eco.Util.Constraints;
using System;

namespace Eco.Util
{
  public class FieldConstraint
  {
    private AConstraint _constraint;
    private string _errorFormat;

    public FieldConstraint(AConstraint constraint, string errorFormat)
    {
      if (constraint == null)
        throw new ArgumentNullException(nameof (constraint));
      if (errorFormat == null)
        throw new ArgumentNullException(nameof (errorFormat));
      this._constraint = constraint;
      this._errorFormat = errorFormat;
    }

    public string FormatError(object value) => string.Format(this._errorFormat, value);

    public bool IsValid(object value) => this._constraint.Test(value);
  }
}
