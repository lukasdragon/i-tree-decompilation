// Decompiled with JetBrains decompiler
// Type: EcoIpedReportGenerator.FlexGridReport
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using System.Collections.Generic;

namespace EcoIpedReportGenerator
{
  public class FlexGridReport
  {
    protected List<GridInfo> m_tables = new List<GridInfo>();
    protected string m_title;

    public List<GridInfo> Data => this.m_tables;

    public string Title
    {
      get => this.m_title;
      set => this.m_title = value;
    }

    public bool ViewStatic { get; set; }
  }
}
