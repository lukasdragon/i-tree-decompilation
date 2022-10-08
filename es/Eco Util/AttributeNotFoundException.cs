// Decompiled with JetBrains decompiler
// Type: Eco.Util.AttributeNotFoundException
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using System;

namespace Eco.Util
{
  public class AttributeNotFoundException : Exception
  {
    public AttributeNotFoundException(string attribute, Type type)
      : base(string.Format("Attribute '{0}' is not a attribute of '{1}'", new object[2]
      {
        (object) attribute,
        (object) type.Name
      }))
    {
    }
  }
}
