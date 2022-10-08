// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.LocalizedDescriptionAttribute
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using System;
using System.ComponentModel;
using System.Reflection;

namespace i_Tree_Eco_v6
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
