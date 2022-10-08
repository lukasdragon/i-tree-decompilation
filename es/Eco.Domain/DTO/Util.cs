// Decompiled with JetBrains decompiler
// Type: Eco.Domain.DTO.Util
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using System;

namespace Eco.Domain.DTO
{
  public static class Util
  {
    public static T? NullIfDefault<T>(T cval, T dval) where T : struct, IComparable => cval.Equals((object) dval) ? new T?() : new T?(cval);

    public static double? NullIfDefault(double cval, double dval) => Math.Abs(cval - dval) > double.Epsilon ? new double?(cval) : new double?();

    public static float? NullIfDefault(float cval, float dval) => (double) Math.Abs(cval - dval) > 1.4012984643248171E-45 ? new float?(cval) : new float?();
  }
}
