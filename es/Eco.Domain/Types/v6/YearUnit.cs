// Decompiled with JetBrains decompiler
// Type: Eco.Domain.Types.v6.YearUnit
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using NHibernate.Type;
using System;

namespace Eco.Domain.Types.v6
{
  [Serializable]
  public class YearUnit : EnumStringType
  {
    public YearUnit()
      : base(typeof (Eco.Domain.v6.YearUnit), 1)
    {
    }

    public override object GetValue(object code)
    {
      if (code == null)
        return (object) string.Empty;
      Eco.Domain.v6.YearUnit yearUnit = (Eco.Domain.v6.YearUnit) code;
      return Enum.IsDefined(typeof (Eco.Domain.v6.YearUnit), (object) yearUnit) ? (object) ((char) yearUnit).ToString() : throw new ArgumentException("Invalid YearUnit");
    }

    public override object GetInstance(object code)
    {
      if (code is string str && str.Length == 1)
      {
        Eco.Domain.v6.YearUnit instance = (Eco.Domain.v6.YearUnit) str[0];
        if (Enum.IsDefined(typeof (Eco.Domain.v6.YearUnit), (object) instance))
          return (object) instance;
      }
      throw new ArgumentException(string.Format("Cannot convert code: {0} to YearUnit", code));
    }
  }
}
