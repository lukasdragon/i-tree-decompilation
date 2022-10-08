// Decompiled with JetBrains decompiler
// Type: EcoIpedReportGenerator.GridInfo
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

namespace EcoIpedReportGenerator
{
  public class GridInfo
  {
    private string m_dm;
    private object m_ds;
    private string m_header;
    private bool m_modify;

    public GridInfo(string tblHeader, object ds, string dm, bool modify)
    {
      this.m_header = tblHeader;
      this.m_ds = ds;
      this.m_dm = dm;
      this.m_modify = modify;
    }

    public bool CanModify => this.m_modify;

    public string DataMember => this.m_dm;

    public object DataSource => this.m_ds;

    public string Header => this.m_header;
  }
}
