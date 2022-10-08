// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.ComboBoxItem
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

namespace i_Tree_Eco_v6.Forms
{
  public class ComboBoxItem
  {
    private readonly string description;

    public ComboBoxItem(short v, string d, bool wo)
    {
      this.Value = v;
      this.description = d;
      this.WeatherOnly = wo;
    }

    public short Value { get; }

    public bool WeatherOnly { get; }

    public override string ToString() => this.description;
  }
}
