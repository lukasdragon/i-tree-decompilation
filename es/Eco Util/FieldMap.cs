// Decompiled with JetBrains decompiler
// Type: Eco.Util.FieldMap
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using System.Collections.Generic;

namespace Eco.Util
{
  public class FieldMap
  {
    private FieldFormat m_activeFormat;
    private string m_srcColumn;
    public static FieldMap Empty = new FieldMap(FieldSpec.Empty);

    public FieldMap(FieldSpec spec)
    {
      this.FieldSpec = spec;
      this.Map = new Dictionary<object, object>();
    }

    public FieldSpec FieldSpec { get; private set; }

    public string SourceColumn
    {
      get => this.m_srcColumn;
      set
      {
        if (this.m_srcColumn != value)
          this.Map = new Dictionary<object, object>();
        this.m_srcColumn = value;
      }
    }

    public FieldFormat ActiveFormat
    {
      get
      {
        if (this.m_activeFormat == null && this.FieldSpec.Formats.Count == 1)
          this.m_activeFormat = this.FieldSpec.Formats[0];
        return this.m_activeFormat;
      }
      set
      {
        this.m_activeFormat = value == null || !this.FieldSpec.Formats.Contains(value) ? (FieldFormat) null : value;
        this.Map = new Dictionary<object, object>();
      }
    }

    public bool NeedsMapped { get; set; }

    public Dictionary<object, object> Map { get; private set; }

    public string Description
    {
      get
      {
        FieldSpec fieldSpec = this.FieldSpec;
        if (string.IsNullOrEmpty(fieldSpec.Description))
          return "-- Not Assigned --";
        return this.FieldSpec.Required ? fieldSpec.Description + "*" : fieldSpec.Description;
      }
    }

    public object Data => this.FieldSpec.Data;

    public string ValueMember => this.FieldSpec.ValueMember;

    public object DefaultValue => this.FieldSpec.DefaultValue;
  }
}
