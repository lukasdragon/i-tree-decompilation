// Decompiled with JetBrains decompiler
// Type: Eco.Util.RecordsetView
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using ADODB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Eco.Util
{
  public class RecordsetView : ITypedList, IList, ICollection, IEnumerable, IDisposable
  {
    private Recordset _rs;
    private Connection _connection;
    private object _lock;
    private PropertyDescriptor[] _props;
    private int _cnt;
    private Dictionary<string, Field> _rsFields;
    private int _pos;
    private bool _disposed;

    public RecordsetView(Recordset rs)
    {
      this._lock = new object();
      this._rs = rs;
      this._connection = (Connection) ((_Recordset) this._rs).ActiveConnection;
      this._cnt = ((_Recordset) this._rs).RecordCount;
      this._rsFields = new Dictionary<string, Field>();
      foreach (Field field in ((_Recordset) this._rs).Fields)
        this._rsFields[field.Name] = field;
      if (this._cnt > 0)
      {
        this._pos = 0;
        ((_Recordset) this._rs).Move(0, (object) BookmarkEnum.adBookmarkFirst);
      }
      else
        this._pos = -1;
    }

    public Recordset Recordset => this._rs;

    public PropertyDescriptorCollection GetItemProperties(
      PropertyDescriptor[] listAccessors)
    {
      if (this._props == null)
      {
        this._props = new PropertyDescriptor[((_Recordset) this.Recordset).Fields.Count];
        int num = 0;
        foreach (Field field in ((_Recordset) this.Recordset).Fields)
        {
          bool nullable = (field.Attributes & 2) > 0;
          Type type = RecordsetView.ConvertADOType(field.Type, nullable);
          this._props[num++] = (PropertyDescriptor) new RecordsetView.RecordsetFieldPropertyDescriptor(field, type);
        }
      }
      return new PropertyDescriptorCollection(this._props);
    }

    public string GetListName(PropertyDescriptor[] listAccessors) => (string) null;

    public int Add(object value) => throw new NotImplementedException();

    public void Clear() => throw new NotImplementedException();

    public bool Contains(object value) => throw new NotImplementedException();

    public int IndexOf(object value) => !(value is RecordsetRow recordsetRow) ? -1 : recordsetRow.Position;

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
      get => index >= 0 && index < this._cnt ? (object) new RecordsetRow(this, index) : throw new ArgumentException(nameof (index));
      set
      {
      }
    }

    public void CopyTo(Array array, int index)
    {
    }

    public int Count => this._cnt;

    public bool IsSynchronized => true;

    public object SyncRoot => this._lock;

    public IEnumerator GetEnumerator() => (IEnumerator) new RecordsetView.RecordsetViewEnumerator(this);

    public static Type ConvertADOType(DataTypeEnum dt, bool nullable)
    {
      Type type;
      switch (dt)
      {
        case DataTypeEnum.adSmallInt:
          type = typeof (short);
          break;
        case DataTypeEnum.adInteger:
          type = typeof (int);
          break;
        case DataTypeEnum.adSingle:
          type = typeof (float);
          break;
        case DataTypeEnum.adDouble:
          type = typeof (double);
          break;
        case DataTypeEnum.adCurrency:
        case DataTypeEnum.adDecimal:
        case DataTypeEnum.adNumeric:
          type = typeof (Decimal);
          break;
        case DataTypeEnum.adDate:
        case DataTypeEnum.adFileTime:
        case DataTypeEnum.adDBDate:
        case DataTypeEnum.adDBTime:
        case DataTypeEnum.adDBTimeStamp:
          type = typeof (DateTime);
          break;
        case DataTypeEnum.adBSTR:
        case DataTypeEnum.adVarChar:
        case DataTypeEnum.adLongVarChar:
        case DataTypeEnum.adVarWChar:
        case DataTypeEnum.adLongVarWChar:
          type = typeof (string);
          break;
        case DataTypeEnum.adBoolean:
          type = typeof (bool);
          break;
        case DataTypeEnum.adTinyInt:
        case DataTypeEnum.adUnsignedTinyInt:
          type = typeof (byte);
          break;
        case DataTypeEnum.adUnsignedSmallInt:
          type = typeof (ushort);
          break;
        case DataTypeEnum.adUnsignedInt:
          type = typeof (uint);
          break;
        case DataTypeEnum.adBigInt:
          type = typeof (long);
          break;
        case DataTypeEnum.adUnsignedBigInt:
          type = typeof (ulong);
          break;
        case DataTypeEnum.adGUID:
          type = typeof (Guid);
          break;
        case DataTypeEnum.adBinary:
        case DataTypeEnum.adVarBinary:
        case DataTypeEnum.adLongVarBinary:
          type = typeof (byte[]);
          break;
        case DataTypeEnum.adChar:
        case DataTypeEnum.adWChar:
          type = typeof (char);
          break;
        default:
          type = typeof (object);
          break;
      }
      if (nullable && type.IsValueType)
        type = typeof (Nullable<>).MakeGenericType(type);
      return type;
    }

    internal RecordsetRow GetRow(int index) => index >= 0 && index < this._cnt ? new RecordsetRow(this, index) : throw new ArgumentException(nameof (index));

    internal object GetFieldValue(int index, string field)
    {
      if (index < 0 || index > this._cnt)
        throw new AggregateException(nameof (index));
      if (field == null)
        throw new AggregateException(nameof (field));
      lock (this._lock)
      {
        if (((_Recordset) this._rs).State == 0)
          return (object) DBNull.Value;
        ((_Recordset) this._rs).Move(index - this._pos, (object) BookmarkEnum.adBookmarkCurrent);
        this._pos = index;
        try
        {
          return this._rsFields[field].Value;
        }
        catch (COMException ex)
        {
          if (ex.ErrorCode == -2147217887)
            return (object) DBNull.Value;
          throw;
        }
      }
    }

    internal void SetFieldValue(int index, string field, object value)
    {
      if (index < 0 || index > ((_Recordset) this._rs).RecordCount)
        throw new AggregateException(nameof (index));
      if (field == null)
        throw new AggregateException(nameof (field));
      lock (this._lock)
      {
        if (((_Connection) this._connection).State == 0)
          return;
        ((_Recordset) this._rs).Move(index, (object) BookmarkEnum.adBookmarkFirst);
        ((_Recordset) this._rs).Fields[(object) field].Value = value;
      }
    }

    public void Dispose() => this.Dispose(true);

    protected virtual void Dispose(bool disposing)
    {
      if (this._disposed)
        return;
      if (disposing && this._rs != null && ((_Recordset) this._rs).State != 0)
      {
        ((_Recordset) this._rs).Close();
        this._rs = (Recordset) null;
      }
      this._disposed = true;
    }

    private class RecordsetViewEnumerator : IEnumerator<RecordsetRow>, IDisposable, IEnumerator
    {
      private int _pos;
      private RecordsetView _rv;

      public RecordsetViewEnumerator(RecordsetView rv)
      {
        this._rv = rv;
        this._pos = -1;
      }

      public RecordsetRow Current => this._rv[this._pos] as RecordsetRow;

      public void Dispose()
      {
      }

      object IEnumerator.Current => (object) this.Current;

      public bool MoveNext()
      {
        ++this._pos;
        return this._pos < this._rv.Count;
      }

      public void Reset() => this._pos = -1;
    }

    private class RecordsetFieldPropertyDescriptor : PropertyDescriptor
    {
      private Field _field;
      private Type _type;
      private string _fname;
      private PropertyDescriptor _pd;

      public RecordsetFieldPropertyDescriptor(Field field, Type type)
        : base(field.Name, (Attribute[]) null)
      {
        this._fname = field.Name;
        this._field = field;
        this._type = type;
        this._pd = TypeDescriptor.GetProperties((object) field).Find("Value", true);
      }

      public override Type ComponentType => typeof (RecordsetRow);

      public override Type PropertyType => this._type;

      public override bool IsReadOnly => this._pd == null || this._pd.IsReadOnly;

      public override bool SupportsChangeEvents => this._pd != null && this._pd.SupportsChangeEvents;

      public override bool CanResetValue(object component) => false;

      public override object GetValue(object component) => component is RecordsetRow recordsetRow ? recordsetRow[this._fname] : (object) DBNull.Value;

      public override void ResetValue(object component)
      {
      }

      public override bool ShouldSerializeValue(object component) => this._pd.ShouldSerializeValue((object) this._field);

      public override void SetValue(object component, object value)
      {
        if (!(component is RecordsetRow recordsetRow))
          return;
        recordsetRow[this._fname] = value;
      }
    }
  }
}
