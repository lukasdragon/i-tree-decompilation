// Decompiled with JetBrains decompiler
// Type: DataBinding
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

public class DataBinding
{
  private int m_flags;
  public object DataSource;
  public string DisplayMember;
  public string ValueMember;
  public string Description;
  public object Value;

  public DataBinding(int flags) => this.m_flags = flags;

  public bool BindableFor(BindingFor b) => true;
}
