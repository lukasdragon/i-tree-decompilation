// Decompiled with JetBrains decompiler
// Type: IPED.Domain.Attributes.LocalizedDescriptionAttribute
// Assembly: IPED.Domain, Version=1.1.6145.0, Culture=neutral, PublicKeyToken=null
// MVID: A1138CF7-F031-4F0B-8D41-1DE13D446B52
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\IPED.Domain.dll

using System;
using System.ComponentModel;
using System.Reflection;

namespace IPED.Domain.Attributes
{
  internal class LocalizedDescriptionAttribute : DescriptionAttribute
  {
    private Type m_resType;
    private string m_prop;

    public LocalizedDescriptionAttribute(Type resType, string property)
    {
      this.m_resType = resType;
      this.m_prop = property;
    }

    public override string Description
    {
      get
      {
        if (this.m_resType != (Type) null)
        {
          PropertyInfo property = this.m_resType.GetProperty(this.m_prop, BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
          if (property != (PropertyInfo) null)
            return property.GetValue((object) null, (object[]) null).ToString();
        }
        return this.m_prop;
      }
    }
  }
}
