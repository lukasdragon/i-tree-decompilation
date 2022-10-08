// Decompiled with JetBrains decompiler
// Type: Eco.Util.DataBindingList`1
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using DaveyTree.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Eco.Util
{
  public class DataBindingList<T> : 
    BindingList<T>,
    IDataBindingList,
    IBindingList,
    IList,
    ICollection,
    IEnumerable
    where T : class
  {
    private bool m_isSorted;
    private ListSortDirection m_sortDirection;
    private PropertyDescriptor m_sortProperty;
    private Dictionary<Type, IComparer> m_typeComparers;
    private Dictionary<string, IComparer> m_propertyComparers;

    public event EventHandler<ListChangedEventArgs> BeforeRemove;

    public DataBindingList()
    {
      this.m_typeComparers = new Dictionary<Type, IComparer>();
      this.m_propertyComparers = new Dictionary<string, IComparer>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    }

    public DataBindingList(IList<T> list)
      : base(list)
    {
      this.m_typeComparers = new Dictionary<Type, IComparer>();
      this.m_propertyComparers = new Dictionary<string, IComparer>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    }

    public bool Sortable { get; set; }

    protected override bool SupportsSortingCore => this.Sortable;

    protected override bool IsSortedCore => this.m_isSorted;

    protected override ListSortDirection SortDirectionCore => this.m_sortDirection;

    protected override PropertyDescriptor SortPropertyCore => this.m_sortProperty;

    protected override void RemoveSortCore()
    {
      this.m_sortDirection = ListSortDirection.Ascending;
      this.m_sortProperty = (PropertyDescriptor) null;
      this.m_isSorted = false;
    }

    protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
    {
      this.m_sortProperty = prop;
      this.m_sortDirection = direction;
      if (!(this.Items is List<T> items))
        return;
      items.Sort(new Comparison<T>(this.Compare));
      this.m_isSorted = true;
      this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
    }

    protected override void RemoveItem(int index)
    {
      if (this.BeforeRemove != null)
        this.BeforeRemove((object) this, new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
      base.RemoveItem(index);
    }

    private int Compare(T lhs, T rhs)
    {
      int num = this.OnComparison(lhs, rhs);
      if (this.m_sortDirection == ListSortDirection.Descending)
        num = -num;
      return num;
    }

    private int OnComparison(T lhs, T rhs)
    {
      object x = (object) lhs == null ? (object) null : this.m_sortProperty.GetValue((object) lhs);
      object y = (object) rhs == null ? (object) null : this.m_sortProperty.GetValue((object) rhs);
      IComparer comparer = (IComparer) null;
      if (x == null)
        return y != null ? -1 : 0;
      if (y == null)
        return 1;
      if (x is IComparable)
        return ((IComparable) x).CompareTo(y);
      if (this.m_propertyComparers.TryGetValue(this.m_sortProperty.Name, out comparer))
        return comparer.Compare(x, y);
      Type key = x.GetType();
      while (!this.m_typeComparers.TryGetValue(key, out comparer))
      {
        key = key.BaseType;
        if (!(key != (Type) null))
          return x.Equals(y) ? 0 : x.ToString().CompareTo(y.ToString());
      }
      return comparer.Compare(x, y);
    }

    public void AddComparer<C>(IComparer comparer) => this.m_typeComparers[typeof (C)] = comparer;

    public void AddComparer<C>(Expression<Func<C, object>> expProperty, IComparer comparer) => this.m_propertyComparers[TypeHelper.NameOf<C>(expProperty)] = comparer;

    public void AddComparer(string property, IComparer comparer) => this.m_propertyComparers[property] = comparer;
  }
}
