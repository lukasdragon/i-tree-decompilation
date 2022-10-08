// Decompiled with JetBrains decompiler
// Type: Eco.Util.PropertyComparer`1
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Eco.Util
{
  public class PropertyComparer<T> : IComparer, IComparer<T>
  {
    private Func<T, object> _function;

    public PropertyComparer(string property)
    {
      ParameterExpression parameterExpression;
      this._function = Expression.Lambda<Func<T, object>>((Expression) Expression.Convert((Expression) Expression.Property((Expression) parameterExpression, property), typeof (object)), parameterExpression).Compile();
    }

    public PropertyComparer(Func<T, object> function) => this._function = function;

    public int Compare(T x, T y) => (object) x == null ? ((object) y != null ? -1 : 0) : ((object) y == null ? 1 : new ObjectComparer().Compare(this._function(x), this._function(y)));

    public int Compare(object x, object y) => this.Compare((T) x, (T) y);
  }
}
