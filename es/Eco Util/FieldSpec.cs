// Decompiled with JetBrains decompiler
// Type: Eco.Util.FieldSpec
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using System;
using System.Collections.Generic;

namespace Eco.Util
{
  public class FieldSpec
  {
    private List<FieldFormat> m_formats;
    private List<FieldConstraint> m_constraints;
    private Type _type;
    public static FieldSpec Empty = new FieldSpec(string.Empty, string.Empty, false);

    public FieldSpec(string field, string description, bool required)
    {
      this.Field = field;
      this.Description = description;
      this.Required = required;
      this.m_formats = new List<FieldFormat>();
      this.m_constraints = new List<FieldConstraint>();
    }

    public FieldSpec(string field, string description, bool required, Type type)
      : this(field, description, required)
    {
      this._type = type;
    }

    public virtual List<FieldFormat> Formats
    {
      get
      {
        if (this.m_formats.Count == 0 && this._type != (Type) null)
          this.m_formats.Add(new FieldFormat(this._type));
        return new List<FieldFormat>((IEnumerable<FieldFormat>) this.m_formats);
      }
    }

    public FieldSpec SetData(object data, string valueMember)
    {
      this.Data = data;
      this.ValueMember = valueMember;
      return this;
    }

    public FieldSpec SetData(object data) => this.SetData(data, (string) null);

    public FieldSpec SetNullValue(object nullValue)
    {
      this.NullValue = nullValue;
      return this;
    }

    public FieldSpec SetDefaultValue(object defaultValue)
    {
      this.DefaultValue = defaultValue;
      return this;
    }

    public FieldSpec AddFormat(FieldFormat format)
    {
      this.m_formats.Add(format);
      return this;
    }

    public FieldSpec AddConstraint(FieldConstraint constraint)
    {
      this.m_constraints.Add(constraint);
      return this;
    }

    public FieldSpec RemoveFormat(FieldFormat format)
    {
      this.m_formats.Remove(format);
      return this;
    }

    public string Field { get; private set; }

    public string Description { get; private set; }

    public bool Required { get; private set; }

    public object Data { get; private set; }

    public string ValueMember { get; private set; }

    public object NullValue { get; private set; }

    public object DefaultValue { get; private set; }

    public List<FieldConstraint> Constraints => new List<FieldConstraint>((IEnumerable<FieldConstraint>) this.m_constraints);
  }
}
