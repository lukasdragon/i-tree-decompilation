// Decompiled with JetBrains decompiler
// Type: Eco.Util.RejectedDataList
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Eco.Util
{
  public class RejectedDataList : IList, ICollection, IEnumerable, ITypedList
  {
    private DataAnalyzer _analyzer;
    private int[] _rowIndexes;
    private int _length;

    public RejectedDataList(DataAnalyzer analyzer, int[] rowIndexes, int length)
    {
      this._analyzer = analyzer;
      this._rowIndexes = rowIndexes;
      this._length = length;
    }

    public int Add(object value) => throw new NotImplementedException();

    public void Clear()
    {
    }

    public bool Contains(object value) => throw new NotImplementedException();

    public int IndexOf(object value) => throw new NotImplementedException();

    public void Insert(int index, object value)
    {
    }

    public bool IsFixedSize => true;

    public bool IsReadOnly => true;

    public void Remove(object value)
    {
    }

    public void RemoveAt(int index)
    {
    }

    public object this[int index]
    {
      get
      {
        int rowIndex = this._rowIndexes[index];
        return (object) new KeyValuePair<int, string>(rowIndex + 1, this._analyzer.GetError(rowIndex));
      }
      set
      {
      }
    }

    public void CopyTo(Array array, int index) => throw new NotImplementedException();

    public int Count => this._length;

    public bool IsSynchronized => false;

    public object SyncRoot => this._analyzer.SyncRoot;

    public IEnumerator GetEnumerator() => (IEnumerator) new RejectedDataList.RejectedDataEnumerator(this);

    public PropertyDescriptorCollection GetItemProperties(
      PropertyDescriptor[] listAccessors)
    {
      return TypeDescriptor.GetProperties(typeof (KeyValuePair<int, string>));
    }

    public string GetListName(PropertyDescriptor[] listAccessors) => typeof (KeyValuePair<int, string>).Name;

    private class RejectedDataEnumerator : IEnumerator<object>, IDisposable, IEnumerator
    {
      private RejectedDataList _list;
      private int _pos;

      public RejectedDataEnumerator(RejectedDataList list)
      {
        this._list = list;
        this._pos = -1;
      }

      public object Current
      {
        get
        {
          try
          {
            return this._list[this._pos];
          }
          catch (IndexOutOfRangeException ex)
          {
            throw new InvalidOperationException();
          }
        }
      }

      public void Dispose()
      {
      }

      public bool MoveNext()
      {
        ++this._pos;
        return this._pos < this._list.Count;
      }

      public void Reset() => this._pos = -1;
    }
  }
}
