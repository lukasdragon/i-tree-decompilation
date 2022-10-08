// Decompiled with JetBrains decompiler
// Type: Eco.Util.ImportedDataList
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Eco.Util
{
  internal class ImportedDataList : ITypedList, IList, ICollection, IEnumerable
  {
    private DataAnalyzer _analyzer;
    private int[] _rowIndexes;
    private int _length;

    public ImportedDataList(DataAnalyzer analyzer, int[] rowIndexes, int length)
    {
      this._analyzer = analyzer;
      this._rowIndexes = rowIndexes;
      this._length = length;
    }

    public int Add(object value) => throw new NotImplementedException();

    public void Clear() => throw new NotImplementedException();

    public bool Contains(object value) => throw new NotImplementedException();

    public int IndexOf(object value) => throw new NotImplementedException();

    public void Insert(int index, object value) => throw new NotImplementedException();

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
        if (index < 0 || index >= this._length)
          throw new ArgumentException(nameof (index));
        return this._analyzer.Process(this._rowIndexes[index]);
      }
      set
      {
      }
    }

    public void CopyTo(Array array, int index)
    {
    }

    public int Count => this._length;

    public bool IsSynchronized => false;

    public object SyncRoot => this._analyzer.SyncRoot;

    public IEnumerator GetEnumerator() => (IEnumerator) new ImportedDataList.ImportedDataEnumerator(this);

    public PropertyDescriptorCollection GetItemProperties(
      PropertyDescriptor[] listAccessors)
    {
      return this._analyzer.GetItemProperties(listAccessors);
    }

    public string GetListName(PropertyDescriptor[] listAccessors) => this._analyzer.GetListName(listAccessors);

    private class ImportedDataEnumerator : IEnumerator<object>, IDisposable, IEnumerator
    {
      private ImportedDataList _list;
      private int _pos;

      public ImportedDataEnumerator(ImportedDataList list)
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
