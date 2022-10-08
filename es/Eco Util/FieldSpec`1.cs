// Decompiled with JetBrains decompiler
// Type: Eco.Util.FieldSpec`1
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using DaveyTree.Core;
using System;
using System.Linq.Expressions;

namespace Eco.Util
{
  public class FieldSpec<T> : FieldSpec
  {
    public FieldSpec(Expression<Func<T, object>> expression, string description, bool required)
      : base(TypeHelper.NameOf<T>(expression), description, required, TypeHelper.TypeOf<T>(expression))
    {
    }
  }
}
