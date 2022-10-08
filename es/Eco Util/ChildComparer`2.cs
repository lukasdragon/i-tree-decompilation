// Decompiled with JetBrains decompiler
// Type: Eco.Util.ChildComparer`2
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Eco.Util
{
  public class ChildComparer<T, V> : IComparer, IComparer<T>
  {
    private Func<T, V> _function;
    private IComparer<V> _childComparer;

    public ChildComparer(string property, IComparer<V> childComparer)
    {
      ParameterExpression parameterExpression;
      this._function = Expression.Lambda<Func<T, V>>((Expression) Expression.Convert((Expression) Expression.Property((Expression) parameterExpression, property), typeof (V)), parameterExpression).Compile();
      this._childComparer = childComparer;
    }

    public ChildComparer(Func<T, V> function, IComparer<V> childComparer)
    {
      this._function = function;
      this._childComparer = childComparer;
    }

    public int Compare(T x, T y) => (object) x == null ? ((object) y != null ? -1 : 0) : ((object) y == null ? -1 : this._childComparer.Compare(this._function(x), this._function(y)));

    public int Compare(object x, object y) => this.Compare((T) x, (T) y);
  }
}
