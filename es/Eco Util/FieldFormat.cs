// Decompiled with JetBrains decompiler
// Type: Eco.Util.FieldFormat
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using System;

namespace Eco.Util
{
  public class FieldFormat
  {
    public string Name { get; private set; }

    public Type FormatType { get; private set; }

    public string Property { get; private set; }

    public FieldFormat(Type type, string name, string prop)
    {
      this.FormatType = type;
      this.Name = name;
      this.Property = prop;
    }

    public FieldFormat(Type type)
      : this(type, (string) null, (string) null)
    {
    }
  }
}
