// Decompiled with JetBrains decompiler
// Type: Misc
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using System;
using System.Data;

public static class Misc
{
  public static bool IsNumericColumn(this DataColumn col) => Misc.IsNumericType(col.DataType);

  public static bool IsNumericType(Type type)
  {
    if (type == (Type) null)
      return false;
    switch (Type.GetTypeCode(type))
    {
      case TypeCode.Object:
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Nullable<>) && Misc.IsNumericType(Nullable.GetUnderlyingType(type));
      case TypeCode.SByte:
      case TypeCode.Byte:
      case TypeCode.Int16:
      case TypeCode.UInt16:
      case TypeCode.Int32:
      case TypeCode.UInt32:
      case TypeCode.Int64:
      case TypeCode.UInt64:
      case TypeCode.Single:
      case TypeCode.Double:
      case TypeCode.Decimal:
        return true;
      default:
        return false;
    }
  }
}
