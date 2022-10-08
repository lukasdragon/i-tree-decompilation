// Decompiled with JetBrains decompiler
// Type: Eco.Domain.Attributes.LocalizedDescriptionAttribute
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using System;
using System.ComponentModel;
using System.Reflection;

namespace Eco.Domain.Attributes
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
