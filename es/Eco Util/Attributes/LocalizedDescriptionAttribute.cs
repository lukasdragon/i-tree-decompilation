// Decompiled with JetBrains decompiler
// Type: Eco.Util.Attributes.LocalizedDescriptionAttribute
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using System;
using System.ComponentModel;
using System.Reflection;

namespace Eco.Util.Attributes
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
