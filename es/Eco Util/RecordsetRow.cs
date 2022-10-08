// Decompiled with JetBrains decompiler
// Type: Eco.Util.RecordsetRow
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

namespace Eco.Util
{
  public class RecordsetRow
  {
    private RecordsetView _rv;
    private int _position;

    internal RecordsetRow(RecordsetView rv, int position)
    {
      this._rv = rv;
      this._position = position;
    }

    internal int Position => this._position;

    public object this[string field]
    {
      get => this._rv.GetFieldValue(this._position, field);
      set => this._rv.SetFieldValue(this._position, field, value);
    }
  }
}
