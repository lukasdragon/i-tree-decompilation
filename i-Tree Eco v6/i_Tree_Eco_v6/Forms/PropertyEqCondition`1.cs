// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.PropertyEqCondition`1
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using System.Reflection;

namespace i_Tree_Eco_v6.Forms
{
  internal class PropertyEqCondition<T> : ConditionBase
  {
    private object _valueToCompare;
    private T _object;
    private string _property;

    public PropertyEqCondition(T obj, string strProperty, object valueToCompare)
    {
      this._object = obj;
      this._property = strProperty;
      this._valueToCompare = valueToCompare;
    }

    public override bool Test()
    {
      PropertyInfo property = typeof (T).GetProperty(this._property);
      if (!(property != (PropertyInfo) null))
        return false;
      object obj = property.GetValue((object) this._object, (object[]) null);
      return obj != null ? obj.Equals(this._valueToCompare) : this._valueToCompare == null;
    }
  }
}
