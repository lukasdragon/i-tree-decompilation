// Decompiled with JetBrains decompiler
// Type: Eco.Util.FieldMapList
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

namespace Eco.Util
{
  public class FieldMapList
  {
    private System.Collections.Generic.List<FieldMap> _mapList;

    public FieldMapList(System.Collections.Generic.List<FieldSpec> list)
    {
      this._mapList = new System.Collections.Generic.List<FieldMap>();
      foreach (FieldSpec spec in list)
        this._mapList.Add(new FieldMap(spec));
    }

    public FieldMapList() => this._mapList = new System.Collections.Generic.List<FieldMap>();

    public System.Collections.Generic.List<FieldMap> List => this._mapList;

    public FieldMap GetFieldMapFromSource(string column)
    {
      if (!string.IsNullOrEmpty(column))
      {
        foreach (FieldMap map in this._mapList)
        {
          if (map.SourceColumn == column)
            return map;
        }
      }
      return (FieldMap) null;
    }

    public FieldMap GetFieldMapFromSpec(FieldSpec spec)
    {
      if (spec != null)
      {
        foreach (FieldMap map in this._mapList)
        {
          if (map.FieldSpec == spec)
            return map;
        }
      }
      return (FieldMap) null;
    }

    public System.Collections.Generic.List<FieldMap> GetUnMatchedFields(string colName)
    {
      System.Collections.Generic.List<FieldMap> unMatchedFields = new System.Collections.Generic.List<FieldMap>();
      if (!string.IsNullOrEmpty(colName))
      {
        unMatchedFields.Add(FieldMap.Empty);
        foreach (FieldMap map in this._mapList)
        {
          if (string.IsNullOrEmpty(map.SourceColumn) || map.SourceColumn == colName)
            unMatchedFields.Add(map);
        }
      }
      return unMatchedFields;
    }

    public System.Collections.Generic.List<FieldMap> GetMatchedFields()
    {
      System.Collections.Generic.List<FieldMap> matchedFields = new System.Collections.Generic.List<FieldMap>();
      foreach (FieldMap map in this._mapList)
      {
        if (!string.IsNullOrEmpty(map.SourceColumn))
          matchedFields.Add(map);
      }
      return matchedFields;
    }

    public System.Collections.Generic.List<FieldMap> GetMatchedFieldsToMap()
    {
      System.Collections.Generic.List<FieldMap> matchedFieldsToMap = new System.Collections.Generic.List<FieldMap>();
      foreach (FieldMap map in this._mapList)
      {
        if (!string.IsNullOrEmpty(map.SourceColumn) && map.NeedsMapped)
          matchedFieldsToMap.Add(map);
      }
      return matchedFieldsToMap;
    }
  }
}
