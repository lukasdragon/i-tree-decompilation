// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.SAS.SASDictionary`2
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using System;
using System.Collections.Generic;

namespace i_Tree_Eco_v6.SAS
{
  public class SASDictionary<T1, T2> : Dictionary<T1, T2>
  {
    private string keyName;

    public SASDictionary(string keyName) => this.keyName = keyName;

    public SASDictionary() => this.keyName = "";

    public void setKeyName(string keyName) => this.keyName = keyName;

    public new void Add(T1 key, T2 value)
    {
      try
      {
        base.Add(key, value);
      }
      catch (Exception ex)
      {
        throw new Exception(key.ToString() + " as a key has already been added in " + this.keyName);
      }
    }

    public new T2 this[T1 key]
    {
      get
      {
        try
        {
          return base[key];
        }
        catch (Exception ex)
        {
          throw new Exception(key.ToString() + " was not present in " + this.keyName);
        }
      }
      set
      {
        try
        {
          base[key] = value;
        }
        catch (Exception ex)
        {
          throw new Exception(key.ToString() + " does not exist in " + this.keyName);
        }
      }
    }
  }
}
