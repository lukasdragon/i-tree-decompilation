// Decompiled with JetBrains decompiler
// Type: Eco.Util.AttributePropertyComparer`2
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Eco.Util
{
  public class AttributePropertyComparer<T, A> : IComparer<T>, IComparer
  {
    private readonly PropertyComparer _pc;
    private readonly string _prop;
    private readonly Type _attrType;
    private readonly Type _objType;

    public AttributePropertyComparer(string attr_prop)
      : this((string) null, attr_prop)
    {
    }

    public AttributePropertyComparer(string property, string attr_prop)
    {
      this._pc = new PropertyComparer(attr_prop);
      this._prop = property;
      this._attrType = typeof (A);
      this._objType = typeof (T);
    }

    public int Compare(T x, T y) => this._pc.Compare((object) this.GetAttribute((object) x), (object) this.GetAttribute((object) y));

    private A GetAttribute(object o)
    {
      A attribute = default (A);
      FieldInfo fieldInfo = (FieldInfo) null;
      if (this._objType.IsEnum)
        fieldInfo = this._objType.GetField(o.ToString());
      else if (this._prop != null)
        fieldInfo = this._objType.GetField(this._prop);
      if (fieldInfo != (FieldInfo) null)
      {
        object[] customAttributes = fieldInfo.GetCustomAttributes(this._attrType, false);
        if (customAttributes.Length != 0)
          attribute = (A) customAttributes[0];
      }
      return attribute;
    }

    public int Compare(object x, object y) => this.Compare((T) x, (T) y);
  }
}
