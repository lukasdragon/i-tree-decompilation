// Decompiled with JetBrains decompiler
// Type: Eco.Util.ImportSpec
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Eco.Util
{
  public class ImportSpec
  {
    private PropertyDescriptorCollection _props;

    public ImportSpec(Type listType)
    {
      this.ListType = listType;
      this.FieldsSpecs = new List<FieldSpec>();
      this._props = TypeDescriptor.GetProperties(listType);
    }

    public List<FieldSpec> FieldsSpecs { get; private set; }

    public Type ListType { get; private set; }

    public PropertyDescriptorCollection Properties => this._props;

    public int RecordCount { get; set; }
  }
}
