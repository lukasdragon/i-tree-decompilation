// Decompiled with JetBrains decompiler
// Type: Eco.Util.PropertyNotFoundException
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using System;

namespace Eco.Util
{
  public class PropertyNotFoundException : Exception
  {
    public PropertyNotFoundException(string property, Type type)
      : base(string.Format("Property '{0}' is not a property of '{1}'", new object[2]
      {
        (object) property,
        (object) type.Name
      }))
    {
    }
  }
}
